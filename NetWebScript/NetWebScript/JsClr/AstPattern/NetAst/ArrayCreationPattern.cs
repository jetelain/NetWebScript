using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class ArrayCreationPattern : NetPatternBase
    {
        public NetPatternBase Size { get; set; }

        public override PatternMatch Visit(Ast.ArrayCreationExpression arrayCreationExpression)
        {
            PatternMatch match = new PatternMatch();
            if (Size.Test(arrayCreationExpression.Size, match) && arrayCreationExpression.Initialize == null)
            {
                return match;
            }
            return null;
        }
    }
}
