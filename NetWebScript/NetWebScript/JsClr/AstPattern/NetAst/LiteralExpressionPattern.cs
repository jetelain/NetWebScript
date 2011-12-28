using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class LiteralExpressionPattern : NetPatternBase
    {
        internal object Value { get; set; }

        public override PatternMatch Visit(Ast.LiteralExpression literalExpression)
        {
            if (object.Equals(literalExpression.Value, Value))
            {
                return new PatternMatch();
            }
            return null;
        }
    }
}
