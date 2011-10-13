using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IRootInvoker : IStatementVisitor<JsToken>
    {
    }
}
