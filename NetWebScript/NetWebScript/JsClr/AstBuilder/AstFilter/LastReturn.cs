using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class LastReturn : IAstFilter
    {
        #region IAstFilter Members

        public void Visit(MethodAst method)
        {
            if (method.IsVoidMethod())
            {
                ReturnStatement last = method.Statements.Last() as ReturnStatement;
                if (last != null)
                {
                    method.Statements.RemoveAt(method.Statements.Count - 1);
                }
            }
        }

        #endregion
    }
}
