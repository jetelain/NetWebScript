using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IValueSerializer
	{
        JsToken LiteralValue(IValueSerializer type, object value, IRootInvoker converter);
    }
}
