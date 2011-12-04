using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    internal class BinaryFix : AstFilterBase
    {
        public override Statement Visit(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Operator == BinaryOperator.GreaterThan && binaryExpression.Left.GetExpressionType() != null && !binaryExpression.Left.GetExpressionType().IsValueType)
            {
                // XXX: Some operation make Brgt_un operations on references to check null inequality
                // => Consider doing this kind of "fixes" into expression builder base
                binaryExpression.Operator = BinaryOperator.ValueInequality;
            }
            return base.Visit(binaryExpression);
        }
    }
}
