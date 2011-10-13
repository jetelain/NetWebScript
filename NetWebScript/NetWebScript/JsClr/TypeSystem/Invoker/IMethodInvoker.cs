using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IMethodInvoker
	{
        JsToken WriteMethod(IScriptMethodBase method, MethodInvocationExpression methodExpression, IRootInvoker converter);

        JsToken WriteMethodReference(IScriptMethodBase method);
    }
	
}
