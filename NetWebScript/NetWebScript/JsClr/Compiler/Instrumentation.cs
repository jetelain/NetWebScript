using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem;

namespace NetWebScript.JsClr.Compiler
{
    public class Instrumentation
    {

        internal Instrumentation(ModuleCompiler compiler, Type type)
        {
            var instrumenterType = compiler.AddEntryPoint(type);
            EnterMethod = instrumenterType.GetScriptMethod(type.GetMethod("E"));
            LeaveMethod = instrumenterType.GetScriptMethod(type.GetMethod("L"));
            Point = instrumenterType.GetScriptMethod(type.GetMethod("P"));
            Start = instrumenterType.GetScriptMethod(type.GetMethod("Start"));
            CaptureMethodContext = true;
        }

        internal bool CaptureMethodContext { get; set; }

        internal IScriptMethod EnterMethod { get; set; }

        internal IScriptMethod LeaveMethod { get; set; }

        internal IScriptMethod Point { get; set; }

        internal IScriptMethod Start { get; set; }
    }
}
