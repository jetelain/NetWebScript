using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IObjectCreationInvoker
	{
        JsToken WriteObjectCreation(IScriptConstructor ctor, ScriptObjectCreationExpression creationExpression, IRootInvoker converter);
	}
}
