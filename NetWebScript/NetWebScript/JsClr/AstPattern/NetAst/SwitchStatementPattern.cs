using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class SwitchStatementPattern : NetPatternBase
    {
        internal PatternBase<Statement> Value { get; set; }

        public override PatternMatch Visit(Ast.SwitchStatement switchStatement)
        {
            PatternMatch match = new PatternMatch();
            if (Value.Test(switchStatement.Value, match))
            {
                return match;
            }
            return null;
        }
    }
}
