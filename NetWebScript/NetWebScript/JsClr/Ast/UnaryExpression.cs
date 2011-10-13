using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(int? ilOffset, UnaryOperator @operator, Expression operand)
            : base (ilOffset)
        {
            Contract.Requires(operand != null);
            this.Operator = @operator;
            this.Operand = operand;
        }

        public UnaryOperator Operator { get; internal set; }

        public Expression Operand { get; internal set; }

        public override Expression Negate()
        {
            if (Operator == UnaryOperator.LogicalNot)
            {
                return Operand;
            }
            return base.Negate();
        }

        public override Type GetExpressionType()
        {
            if (Operator == UnaryOperator.LogicalNot)
            {
                return typeof(bool);
            }
            return Operand.GetExpressionType();
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", Operator, Operand.ToString() );
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
                return Operand.FirstIlOffset;
            }
        }
    }
}
