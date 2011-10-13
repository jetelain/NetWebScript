using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public class ModuleInfo
    {
        public String Name { get; set; }

        public String Version { get; set; }

        public Uri Uri { get; set; }

    }
}
