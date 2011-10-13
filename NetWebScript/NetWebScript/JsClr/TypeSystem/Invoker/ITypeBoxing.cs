using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.JsBuilder.JsSyntax;

namespace NetWebScript.JsClr.TypeSystem.Invoker
{
    public interface ITypeBoxing
	{
        Expression BoxValue(IScriptType type, BoxExpression boxExpression);

        Expression UnboxValue(IScriptType type, UnboxExpression boxExpression);
    }
}
