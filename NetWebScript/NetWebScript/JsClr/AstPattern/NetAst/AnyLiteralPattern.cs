using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyLiteralPattern : NetPatternBase
    {
        internal Type Type { get; set; }

        internal string Name { get; set; }

        public override PatternMatch Visit(Ast.LiteralExpression literalExpression)
        {
            if (Type == null || (literalExpression.Value != null && literalExpression.Value.GetType() == Type))
            {
                if (Name != null)
                {
                    return new PatternMatch(Name, literalExpression.Value);
                }
                return new PatternMatch();
            }
            return null;
        }
    }
}
