using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface IRootInvoker : IScriptStatementVisitor<JsToken>
    {
    }
}
