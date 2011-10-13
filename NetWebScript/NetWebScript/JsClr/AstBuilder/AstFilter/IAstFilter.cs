using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    interface IAstFilter
    {
        void Visit(MethodAst method);
    }
}
