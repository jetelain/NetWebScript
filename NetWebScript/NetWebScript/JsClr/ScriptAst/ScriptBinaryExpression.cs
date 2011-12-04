
using System;
using System.Collections.Generic;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptBinaryExpression : ScriptExpression
    {
        public ScriptBinaryExpression(int? ilOffset, ScriptBinaryOperator @operator, ScriptExpression right, ScriptExpression left)
            : base(ilOffset)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
        }

        public ScriptBinaryOperator Operator { get; internal set; }

        public ScriptExpression Left { get; internal set; }

        public ScriptExpression Right { get; internal set; }

        public override ScriptExpression Negate()
        {
            switch (Operator)
            {
                case ScriptBinaryOperator.ValueEquality: // a == b becomes a != b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.ValueInequality, Right, Left);

                case ScriptBinaryOperator.ValueInequality: // a != b becomes a == b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.ValueEquality, Right, Left);

                case ScriptBinaryOperator.LogicalOr: // a || b becomes !a && !b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.LogicalAnd, Right.Negate(), Left.Negate());

                case ScriptBinaryOperator.LogicalAnd: // a && b becomes !a || !b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.LogicalOr, Right.Negate(), Left.Negate());

                case ScriptBinaryOperator.LessThan: // a < b becomes a >= b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.GreaterThanOrEqual, Right, Left);

                case ScriptBinaryOperator.LessThanOrEqual: // a <= b becomes a > b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.GreaterThan, Right, Left);

                case ScriptBinaryOperator.GreaterThan: // a > b becomes a <= b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.LessThanOrEqual, Right, Left);

                case ScriptBinaryOperator.GreaterThanOrEqual: // a >= b becomes a < b
                    return new ScriptBinaryExpression(IlOffset, ScriptBinaryOperator.LessThan, Right, Left);

                default:
                    throw new InvalidOperationException();
            }
        }


        public override Type GetExpressionType()
        {
            switch (Operator)
            {
                case ScriptBinaryOperator.ValueEquality: // ==
                case ScriptBinaryOperator.ValueInequality: // !=
                case ScriptBinaryOperator.LogicalOr: // ||
                case ScriptBinaryOperator.LogicalAnd: // &&
                case ScriptBinaryOperator.LessThan: // <
                case ScriptBinaryOperator.LessThanOrEqual: // <=
                case ScriptBinaryOperator.GreaterThan: // >
                case ScriptBinaryOperator.GreaterThanOrEqual: // >=
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
            if (Left is ScriptAssignExpression || Right is ScriptAssignExpression)
            {
                return String.Format("(({0}) {1} ({2}))", Left.ToString(), Operator, Right.ToString());
            }
            return String.Format("({0} {1} {2})", Left.ToString(), Operator, Right.ToString());
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
