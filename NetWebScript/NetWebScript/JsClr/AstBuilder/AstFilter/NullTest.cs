using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    /// <summary>
    /// A faire au fils de l'eau dans ExpressionBuilder ?
    /// </summary>
    class NullTest : AstFilterBase
    {
        public override Statement Visit(ConditionExpression conditionExpression)
        {
            conditionExpression.Condition = FilterCondition(conditionExpression.Condition);
            return base.Visit(conditionExpression);
        }

        public override Statement Visit(IfStatement ifStatement)
        {
            ifStatement.Condition = FilterCondition(ifStatement.Condition);
            return base.Visit(ifStatement);
        }

        public override Statement Visit(WhileStatement whileStatement)
        {
            whileStatement.Condition = FilterCondition(whileStatement.Condition);
            return base.Visit(whileStatement);
        }

        public override Statement Visit(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operator == UnaryOperator.LogicalNot)
            {
                Type type = unaryExpression.Operand.GetExpressionType();
                if (type != typeof(bool))
                {
                    if (!type.IsValueType)
                    {
                        return new BinaryExpression(null, BinaryOperator.ValueEquality, new LiteralExpression(null), unaryExpression.Operand);
                    }
                }
            }
            return base.Visit(unaryExpression);
        }


        private Expression FilterCondition(Expression expr)
        {
            Type type = expr.GetExpressionType();
            if (type != typeof(bool))
            {
                if (!type.IsValueType)
                {
                    return new BinaryExpression(null, BinaryOperator.ValueInequality, new LiteralExpression(null), expr);
                }
            }
            return expr;
        }
    }
}
