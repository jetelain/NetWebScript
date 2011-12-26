using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class NoneTypeBoxing : ITypeBoxing
    {
        public Ast.Expression BoxValue(IScriptType type, Ast.BoxExpression boxExpression)
        {
            return boxExpression.Value;
        }

        public Ast.Expression UnboxValue(IScriptType type, Ast.UnboxExpression boxExpression)
        {
            throw new Exception(string.Format("Cannot unbox vale of type '{0}'", type.Type.FullName));
        }
    }
}
