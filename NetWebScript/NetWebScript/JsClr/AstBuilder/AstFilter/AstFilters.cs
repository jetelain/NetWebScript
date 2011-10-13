using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    public class AstFilters
    {
        public static void Filter(MethodAst method)
        {
            new LastReturn().Visit(method);
            new LiteralsTypeFix().Visit(method);
            new LogicOperations().Visit(method);
            new NullTest().Visit(method);
        }
    }
}
