using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class CastExpression : Expression
    {
        public CastExpression(int? ilOffset, Type type, Expression value)
            : base(ilOffset)
        {
            Contract.Requires(type != null);
            Contract.Requires(value != null);
            this.Type = type;
            this.Value = value;
        }

        public Type Type { get; internal set; }

        public Expression Value { get; internal set; }

        public override Type GetExpressionType()
        {
            return Type;
        }

        public override string ToString()
        {
            return String.Format("({0}){1}",Type.Name,Value.ToString());
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
                return Value.FirstIlOffset;
            }
        }
    }
}
