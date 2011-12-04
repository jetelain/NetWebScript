using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IMethodInvoker
	{
        JsToken WriteMethod(IScriptMethodBase method, ScriptMethodInvocationExpression methodExpression, IRootInvoker converter);

        JsToken WriteMethodReference(IScriptMethodBase method);
    }
	
}
