using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    class LogicOperations : AstFilterBase
    {
        public override Statement Visit(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Operator == BinaryOperator.ValueEquality)
            {
                LiteralExpression right = binaryExpression.Right as LiteralExpression;
                if (right != null && right.Value is bool && (bool)right.Value == false)
                {
                    return (Expression)binaryExpression.Left.Negate().Accept(this);
                }
            }
            return base.Visit(binaryExpression);
        }

        public override Statement Visit(ConditionExpression conditionExpression)
        {
            LiteralExpression then = conditionExpression.Then as LiteralExpression;
            if (then != null && then.Value is bool)
            {
                if ((bool)then.Value)
                {
                    return new BinaryExpression(null, BinaryOperator.LogicalOr, (Expression)conditionExpression.Else.Accept(this), (Expression)conditionExpression.Condition.Accept(this));
                }
                return new BinaryExpression(null, BinaryOperator.LogicalAnd, (Expression)conditionExpression.Else.Accept(this), (Expression)conditionExpression.Condition.Negate().Accept(this));
            }
            return base.Visit(conditionExpression);
        }

    }
}
