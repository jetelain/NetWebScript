using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class IfStatementPattern : NetPatternBase
    {
        internal PatternBase<Statement> Condition { get; set; }

        internal SequencePattern<Statement> Then { get; set; }

        internal SequencePattern<Statement> Else { get; set; }

        public override PatternMatch Visit(Ast.IfStatement ifStatement)
        {
            PatternMatch match = new PatternMatch();
            if (Condition.Test(ifStatement.Condition, match))
            {
                if (!Then.Test(ifStatement.Then, match))
                {
                    return null;
                }
                if (Else != null && !Else.Test(ifStatement.Else, match))
                {
                    return null;
                }
                return match;
            }
            return null;
        }

    }
}
