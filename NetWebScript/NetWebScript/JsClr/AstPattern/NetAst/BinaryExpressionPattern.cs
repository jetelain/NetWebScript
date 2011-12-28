using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class BinaryExpressionPattern : NetPatternBase
    {
        internal NetPatternBase Left { get; set; }

        internal BinaryOperator Operator { get; set; }

        internal NetPatternBase Right { get; set; }

        public override PatternMatch Visit(Ast.BinaryExpression binaryExpression)
        {
            if (binaryExpression.Operator == Operator)
            {
                PatternMatch match = new PatternMatch();
                if (Left.Test(binaryExpression.Left, match) && Right.Test(binaryExpression.Right, match))
                {
                    return match;
                }
            }
            return base.Visit(binaryExpression);
        }
    }
}
