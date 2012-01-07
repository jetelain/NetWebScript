using System;
using System.IO;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.Engine.Script
{
    class ScriptModule : IDebugModule3
    {
        private JSModuleInfo moduleInfo;

        public ScriptModule(JSModuleInfo moduleInfo)
        {
            this.moduleInfo = moduleInfo;
        }

        public int ModuleId { get { return moduleInfo.Id; } }

        public string ModuleFile { get { return moduleInfo.Uri.OriginalString; } }

        public bool HasSymbols { get { return moduleInfo.HasSymbols; } }

        public string SymbolsFile { get { return moduleInfo.SymbolsFileName; } }

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

        internal void UpdateMetadata(JSModuleInfo module)
        {
            moduleInfo = module;
        }
    }
}
