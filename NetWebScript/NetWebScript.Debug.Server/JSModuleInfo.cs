using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSModuleInfo
    {
        private readonly string symbolsFile;
        private readonly int id;

        internal JSModuleInfo ( int id, ModuleInfo module )
        {
            this.id = id;
            symbolsFile = module.Uri.ToString() + ".xml";
            Name = module.Name;
            Uri = module.Uri;
            Version = module.Version;
            Timestamp = module.Timestamp;
        }

        public string Name { get; internal set; }

        public string Version { get; internal set; }

        public Uri Uri { get; internal set; }

        public string Timestamp { get; internal set; }

        public bool HasSymbols { get; internal set; }

        public int Id { get { return id; } }

        public string SymbolsFileName { get { return symbolsFile; } }

    }
}
