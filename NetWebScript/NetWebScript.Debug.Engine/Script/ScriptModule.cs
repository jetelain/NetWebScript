using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using System.IO;
using System.Xml;
using NetWebScript.Debug.Engine.Debug;
using NetWebScript.Debug.Server;
using System.Diagnostics;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Engine.Script
{
    class ScriptModule : IDebugModule3
    {
        private readonly List<DocumentContext> points = new List<DocumentContext>();
        private ModuleMetadata metadata;

        //public ScriptModule(string moduleFile, string symbolsFile)
        //{
        //    this.ModuleFile = moduleFile;
        //    this.SymbolsFile = symbolsFile;
        //    if (symbolsFile != null)
        //    {
        //        LoadPoints(symbolsFile);
        //    }
        //}

        public ScriptModule(JSModuleInfo module)
        {
            this.ModuleFile = module.Uri.ToString();
            this.Uri = module.Uri;
            this.ModuleId = module.Id;
            String symbols = ModuleFile + ".xml";
            try
            {
                LoadPoints(symbols);
                SymbolsFile = symbols;
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Unable to load symbols from {0} : {1}",symbols,e.ToString());
            }
        }

        public int ModuleId { get; private set; }

        private void LoadPoints(string symbolsFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(symbolsFile);
            metadata = ModuleMetadataSerializer.Read(document);
            LoadPoints(metadata);
        }

        private void LoadPoints(ModuleMetadata module)
        {
            var documents = new Dictionary<string, string>();
            foreach (var doc in module.Documents)
            {
                documents.Add(doc.Id, doc.Filename);
            }
            foreach (var type in module.Types)
            {
                foreach (var method in type.Methods)
                {
                    foreach (var point in method.Points)
                    {
                        points.Add(new DocumentContext(
                            this,
                            point.Id,
                            documents[point.DocumentId],
                            point.StartRow, 
                            point.StartCol,
                            point.EndRow,
                            point.EndCol));
                    }
                }
            }
        }

        public IEnumerable<DocumentContext> ResolvePoints(String file, int startCol, int startRow)
        {
            return points.Where(p => 
                p.StartCol == startCol && 
                p.StartRow == startRow && 
                p.FileName.Equals(file, StringComparison.OrdinalIgnoreCase));
        }

        public DocumentContext ResolvePointById(string id)
        {
            return points.FirstOrDefault(p => p.Id == id );
        }

        public MethodBaseMetadata ResolveMethod(string name)
        {
            foreach (var type in metadata.Types)
            {
                foreach (var method in type.Methods)
                {
                    if (method.Id == name)
                    {
                        return method;
                    }
                }
            }
            return null;
        }

        public TypeMetadata ResolveType(string name)
        {
            foreach (var type in metadata.Types)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        public String ModuleFile { get; private set; }

        public bool HasSymbols { get { return SymbolsFile != null; } }

        public String SymbolsFile { get; private set; }

        #region IDebugModule2 Members

        public int GetInfo(enum_MODULE_INFO_FIELDS dwFields, MODULE_INFO[] pinfo)
        {
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_NAME) != 0)
            {
                pinfo[0].m_bstrName = Path.GetFileName(ModuleFile);
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_NAME;
            }
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_URL) != 0)
            {
                pinfo[0].m_bstrUrl = ModuleFile;
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_URL;
            }
            /*if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS) != 0)
            {
                pinfo[0].m_addrLoadAddress = (ulong)this.DebuggedModule.BaseAddress;
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_LOADADDRESS;
            }
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS) != 0)
            {
                // A debugger that actually supports showing the preferred base should crack the PE header and get 
                // that field. This debugger does not do that, so assume the module loaded where it was suppose to.                   
                pinfo[0].m_addrPreferredLoadAddress = (ulong)this.DebuggedModule.BaseAddress;
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_PREFFEREDADDRESS; ;
            }
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_SIZE) != 0)
            {
                pinfo[0].m_dwSize = this.DebuggedModule.Size;
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_SIZE;
            }
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_LOADORDER) != 0)
            {
                pinfo[0].m_dwLoadOrder = this.DebuggedModule.GetLoadOrder();
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_LOADORDER;
            }*/
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_URLSYMBOLLOCATION) != 0)
            {
                pinfo[0].m_bstrUrlSymbolLocation = SymbolsFile;
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_URLSYMBOLLOCATION;
                
            }
            if ((dwFields & enum_MODULE_INFO_FIELDS.MIF_FLAGS) != 0)
            {
                pinfo[0].m_dwModuleFlags = 0;
                if (HasSymbols)
                {
                    pinfo[0].m_dwModuleFlags |= (enum_MODULE_FLAGS.MODULE_FLAG_SYMBOLS);
                }
                pinfo[0].dwValidFields |= enum_MODULE_INFO_FIELDS.MIF_FLAGS;
            }
            return Constants.S_OK;
        }

        [Obsolete]
        public int ReloadSymbols_Deprecated(string pszUrlToSymbols, out string pbstrDebugMessage)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugModule3 Members

        public int GetSymbolInfo(enum_SYMBOL_SEARCH_INFO_FIELDS dwFields, MODULE_SYMBOL_SEARCH_INFO[] pinfo)
        {
            // This engine only supports loading symbols at the location specified in the binary's symbol path location in the PE file and
            // does so only for the primary exe of the debuggee.
            // Therefore, it only displays if the symbols were loaded or not. If symbols were loaded, that path is added.
            pinfo[0].dwValidFields = 1; // SSIF_VERBOSE_SEARCH_INFO;

            if (HasSymbols)
            {
                string symbolPath = "Symbols Loaded - " + SymbolsFile;
                pinfo[0].bstrVerboseSearchInfo = symbolPath;
            }
            else
            {
                string symbolsNotLoaded = "Symbols not loaded";
                pinfo[0].bstrVerboseSearchInfo = symbolsNotLoaded;
            }
            return Constants.S_OK;
        }

        public int IsUserCode(out int pfUser)
        {
            pfUser = HasSymbols ? 1 : 0;
            return Constants.S_OK;   
        }

        public int LoadSymbols()
        {
            return Constants.S_OK;
        }

        public int SetJustMyCodeState(int fIsUserCode)
        {
            return Constants.S_OK;   
        }

        #endregion

        internal DocumentContext GetPoint(string id)
        {
            return points.FirstOrDefault(p => p.Id == id);
        }

        internal IEnumerable<DocumentContext> Points
        {
            get { return points; }
        }

        internal Uri Uri { get; set; }

        internal void UpdateMetadata(JSModuleInfo module)
        {
            // TODO: This ModuleUpdate approach is not really thread safe, it's a temporary solution to avoid VS restart 
            // after each module compilation...

            metadata = null;
            points.Clear();

            string symbols = ModuleFile + ".xml";
            try
            {
                LoadPoints(symbols);
                SymbolsFile = symbols;
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Unable to load symbols from {0} : {1}", symbols, e.ToString());
            }
        }
    }
}
