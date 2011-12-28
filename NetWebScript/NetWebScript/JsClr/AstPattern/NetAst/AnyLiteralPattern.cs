using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyLiteralPattern : NetPatternBase
    {
        internal Type Type { get; set; }

        public override PatternMatch Visit(Ast.LiteralExpression literalExpression)
        {
            if (Type == null || (literalExpression.Value != null && literalExpression.Value.GetType() == Type))
            {
                return new PatternMatch();
            }
            return null;
        }
    }
}
