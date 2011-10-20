using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    internal class OperatorInvoker : IMethodInvoker
    {
        internal static readonly IMethodInvoker Instance = new OperatorInvoker();

        public JsBuilder.JsSyntax.JsToken WriteMethod(IScriptMethodBase method, Ast.MethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return JsToken.Combine(methodExpression.Arguments[0].Accept(converter), method.ImplId, methodExpression.Arguments[1].Accept(converter));
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IScriptMethodBase method)
        {
            throw new NotImplementedException();
        }
    }
}
