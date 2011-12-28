using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AssignExpressionPattern : NetPatternBase
    {
        internal PatternBase<Statement> Target { get; set; }

        internal PatternBase<Statement> Value { get; set; }

        public override PatternMatch Visit(AssignExpression assignExpression)
        {
            PatternMatch match = new PatternMatch();
            if (Target.Test(assignExpression.Target, match) && Value.Test(assignExpression.Value, match))
            {
                return match;
            }
            return null;
        }

    }
}
