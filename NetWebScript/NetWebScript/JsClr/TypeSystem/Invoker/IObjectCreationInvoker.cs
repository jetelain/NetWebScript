using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IObjectCreationInvoker
	{
        JsToken WriteObjectCreation(IScriptConstructor ctor, ObjectCreationExpression creationExpression, IRootInvoker converter);
	}
}
