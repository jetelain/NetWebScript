using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public class ModuleInfo
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public Uri Uri { get; set; }

        public string Timestamp { get; set; }
    }
}
