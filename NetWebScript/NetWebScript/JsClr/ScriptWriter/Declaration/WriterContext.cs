using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Compiler;

namespace NetWebScript.JsClr.ScriptWriter.Declaration
{
    public class WriterContext
    {
        public bool IsPretty { get; set; }

        public Instrumentation Instrumentation { get; set; }

        public IScriptTypeStandardDeclarationWriter StandardDeclarationWriter { get; set; }
    }
}
