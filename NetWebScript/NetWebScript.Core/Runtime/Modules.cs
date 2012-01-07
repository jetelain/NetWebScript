using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.Diagnostics;

namespace NetWebScript.Runtime
{
    [ScriptAvailable, Exported(IgnoreNamespace=true)]
    public static class Modules
    {
        private static readonly JSArray<ModuleInfo> list = new JSArray<ModuleInfo>();

        [DebuggerHidden]
        public static JSArray<ModuleInfo> List
        {
            get { return list; }
        }

        [DebuggerHidden]
        public static void Reg(string name, string version, string filename, string timestamp) 
        {
            var module = new ModuleInfo() { Name= name, Version= version, Filename= filename, Timestamp= timestamp };
            list.Push(module);
        }
    }
}
