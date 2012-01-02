using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Server
{
    internal sealed class JSModule
    {
        private ModuleMetadata metadata;
        private readonly JSModuleInfo infos;
        private readonly List<JSDebugPoint> points = new List<JSDebugPoint>();
        private readonly int id;

        internal JSModule(ModuleInfo module, int id)
        {
            this.infos = new JSModuleInfo(id, module);
            this.id = id;
            try
            {
                LoadMetadata();
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Unable to load symbols from {0} : {1}", infos.SymbolsFileName, e.ToString());
            }
        }

        private void LoadMetadata()
        {
            points.Clear();
            metadata = null;
            infos.HasSymbols = false;
            XmlDocument document = new XmlDocument();
            document.Load(infos.SymbolsFileName);
            metadata = ModuleMetadataSerializer.Read(document);
            infos.HasSymbols = true;
            ListDebugPoints();
        }

        private void ListDebugPoints()
        {
            Contract.Requires(metadata != null);
            Contract.Requires(points.Count == 0);

            var documents = new Dictionary<string,string>();
            foreach (var doc in metadata.Documents)
            {
                documents.Add(doc.Id, doc.Filename);
            }

            foreach (var type in metadata.Types)
            {
                foreach (var method in type.Methods)
                {
                    foreach (var point in method.Points)
                    {
                        points.Add(new JSDebugPoint(
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

        public IEnumerable<JSDebugPoint> FindPoints(string fileName, int startCol, int startRow)
        {
            if (metadata == null)
            {
                return null;
            }
            return points.Where(p =>
                p.StartCol == startCol &&
                p.StartRow == startRow &&
                string.Equals(p.FileName, fileName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<JSDebugPoint> FindPoints(string fileName, int startRow)
        {
            if (metadata == null)
            {
                return null;
            }
            return points.Where(p =>
                p.StartRow == startRow &&
                string.Equals(p.FileName, fileName, StringComparison.OrdinalIgnoreCase));
        }

        public MethodBaseMetadata GetMethodById(string name)
        {
            if (metadata == null)
            {
                return null;
            }
            return metadata.Types.SelectMany(t => t.Methods).FirstOrDefault(m => m.Id == name);
        }

        public TypeMetadata GetTypeById(string name)
        {
            if (metadata == null)
            {
                return null;
            }
            return metadata.Types.FirstOrDefault(t => t.Name == name);
        }

        public JSDebugPoint GetPointById(string id)
        {
            if (metadata == null)
            {
                return null;
            }
            return points.FirstOrDefault(p => p.UId == id);
        }

        public int Id { get { return id; } }

        public Uri ModuleUri { get { return infos.Uri; } }

        internal JSModuleInfo ModuleInfo { get { return infos; } }

        internal void UpdateMetadata(ModuleInfo newInfos)
        {
            lock (this)
            {
                this.infos.Version = newInfos.Version;
                this.infos.Timestamp = newInfos.Timestamp;
                try
                {
                    LoadMetadata();
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("Unable to load symbols from {0} : {1}", infos.SymbolsFileName, e.ToString());
                }
            }
        }

        internal IEnumerable<string> ListSourceFiles()
        {
            if (metadata == null)
            {
                return Enumerable.Empty<string>();
            }
            return metadata.Documents.Select(d => d.Filename);
        }
    }
}
