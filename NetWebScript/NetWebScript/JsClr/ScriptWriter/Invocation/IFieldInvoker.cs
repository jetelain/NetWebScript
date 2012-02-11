using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IFieldInvoker
	{
        JsToken WriteField(IInvocableField field, ScriptFieldReferenceExpression fieldExpression, IRootInvoker converter);
	}
}
