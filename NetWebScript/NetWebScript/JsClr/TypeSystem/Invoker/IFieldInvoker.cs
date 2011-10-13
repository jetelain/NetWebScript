using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IFieldInvoker
	{
        JsToken WriteField(IScriptField field ,FieldReferenceExpression fieldExpression, IRootInvoker converter);
	}
}
