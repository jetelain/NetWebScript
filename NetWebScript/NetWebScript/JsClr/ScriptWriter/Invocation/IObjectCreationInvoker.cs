using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IObjectCreationInvoker
	{
        JsToken WriteObjectCreation(IInvocableConstructor ctor, ScriptObjectCreationExpression creationExpression, IRootInvoker converter);
	}
}
