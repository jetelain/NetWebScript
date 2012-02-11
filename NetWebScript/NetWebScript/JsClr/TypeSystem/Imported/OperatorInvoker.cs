using System;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Imported
{
    internal class OperatorInvoker : IMethodInvoker
    {
        internal static readonly IMethodInvoker Instance = new OperatorInvoker();

        public JsBuilder.JsSyntax.JsToken WriteMethod(IInvocableMethodBase method, ScriptAst.ScriptMethodInvocationExpression methodExpression, IRootInvoker converter)
        {
            return JsToken.Combine(methodExpression.Arguments[0].Accept(converter), method.ImplId, methodExpression.Arguments[1].Accept(converter));
        }

        public JsBuilder.JsSyntax.JsToken WriteMethodReference(IInvocableMethodBase method)
        {
            throw new NotImplementedException();
        }
    }
}
