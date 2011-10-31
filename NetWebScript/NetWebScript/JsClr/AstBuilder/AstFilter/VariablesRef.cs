using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class VariableRef : AstFilterBase
    {
        public override Statement Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            makeByRefVariableExpression.Variable.AllowRef = true;
            return base.Visit(makeByRefVariableExpression);
        }
    }
}
