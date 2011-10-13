using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class UnboxExpression : Expression
    {
        internal UnboxExpression(int ilOffset, Type type, Expression value)
            : base(ilOffset)
        {
            Contract.Requires(type != null);
            Contract.Requires(value != null);
            this.Type = type;
            this.Value = value;
        }

        public Type Type { get; private set; }

        public Expression Value { get; internal set; }

        public override Type GetExpressionType()
        {
            return Type;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int? FirstIlOffset
        {
            get
            {
                return Value.FirstIlOffset;
            }
        }
    }
}
