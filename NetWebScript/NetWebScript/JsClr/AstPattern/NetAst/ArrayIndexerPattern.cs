using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class ArrayIndexerPattern : NetPatternBase
    {
        internal PatternBase<Statement> Target { get; set; }

        internal PatternBase<Statement> Index { get; set; }


        public override PatternMatch Visit(Ast.ArrayIndexerExpression arrayIndexerExpression)
        {
            PatternMatch match = new PatternMatch();
            if (Target.Test(arrayIndexerExpression.Target, match) && Index.Test(arrayIndexerExpression.Index, match))
            {
                return match;
            }
            return null;
        }
    }
}
