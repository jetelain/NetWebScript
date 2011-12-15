using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.Ast
{
    public sealed class Catch
    {
        public Type Type { get; internal set; }
        
        public List<Statement> Body { get; internal set; }

        public bool IsFault { get; set; }
    }
}
