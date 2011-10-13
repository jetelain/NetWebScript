
using System;
using System.Collections.Generic;

namespace NetWebScript.JsClr.Ast
{
    public sealed class BinaryExpression : Expression
    {
        public BinaryExpression(int? ilOffset, BinaryOperator @operator, Expression right, Expression left)
            : base(ilOffset)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
        }

        public BinaryOperator Operator { get; internal set; }

        public Expression Left { get; internal set; }

        public Expression Right { get; internal set; }

        public override Expression Negate()
        {
            switch (Operator)
            {
                case BinaryOperator.ValueEquality: // a == b becomes a != b
                    return new BinaryExpression(IlOffset, BinaryOperator.ValueInequality, Right, Left);

                case BinaryOperator.ValueInequality: // a != b becomes a == b
                    return new BinaryExpression(IlOffset, BinaryOperator.ValueEquality, Right, Left);

                case BinaryOperator.LogicalOr: // a || b becomes !a && !b
                    return new BinaryExpression(IlOffset, BinaryOperator.LogicalAnd, Right.Negate(), Left.Negate());

                case BinaryOperator.LogicalAnd: // a && b becomes !a || !b
                    return new BinaryExpression(IlOffset, BinaryOperator.LogicalOr, Right.Negate(), Left.Negate());

                case BinaryOperator.LessThan: // a < b becomes a >= b
                    return new BinaryExpression(IlOffset, BinaryOperator.GreaterThanOrEqual, Right, Left);

                case BinaryOperator.LessThanOrEqual: // a <= b becomes a > b
                    return new BinaryExpression(IlOffset, BinaryOperator.GreaterThan, Right, Left);

                case BinaryOperator.GreaterThan: // a > b becomes a <= b
                    return new BinaryExpression(IlOffset, BinaryOperator.LessThanOrEqual, Right, Left);

                case BinaryOperator.GreaterThanOrEqual: // a >= b becomes a < b
                    return new BinaryExpression(IlOffset, BinaryOperator.LessThan, Right, Left);

                default:
                    throw new InvalidOperationException();
            }
        }


        public override Type GetExpressionType()
        {
            switch (Operator)
            {
                case BinaryOperator.ValueEquality: // ==
                case BinaryOperator.ValueInequality: // !=
                case BinaryOperator.LogicalOr: // ||
                case BinaryOperator.LogicalAnd: // &&
                case BinaryOperator.LessThan: // <
                case BinaryOperator.LessThanOrEqual: // <=
                case BinaryOperator.GreaterThan: // >
                case BinaryOperator.GreaterThanOrEqual: // >=
                    return typeof(bool);

                default:
                    // FIXME: Use the MSDN OpCodes table
                    Type typeLeft = Left.GetExpressionType();
                    Type typeRight = Right.GetExpressionType();
                    if (typeLeft == typeof(double) || typeRight == typeof(double))
                    {
                        return typeof(double);
                    }
                    if (typeLeft == typeof(float) || typeRight == typeof(float))
                    {
                        return typeof(float);
                    }
                    if (typeLeft == typeof(long) || typeRight == typeof(long))
                    {
                        return typeof(long);
                    }
                    return null;
            }
        }

        public override string ToString()
        {
            if (Left is AssignExpression || Right is AssignExpression)
            {
                return String.Format("(({0}) {1} ({2}))", Left.ToString(), Operator, Right.ToString());
            }
            return String.Format("({0} {1} {2})", Left.ToString(), Operator, Right.ToString());
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override int? FirstIlOffset
        {
            get
            {
                return Left.FirstIlOffset;
            }
        }
    }
}
