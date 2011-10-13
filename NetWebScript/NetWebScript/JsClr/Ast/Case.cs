using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class Case
    {
        public static readonly Object DefaultCase = new Object();

        public Object Value { get; internal set; }

        public List<Statement> Statements { get; internal set; }
    }
}
