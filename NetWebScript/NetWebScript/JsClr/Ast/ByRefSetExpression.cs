using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public class ByRefSetExpression : Expression
    {
        public ByRefSetExpression(int ilOffset, Expression target, Expression value)
            : base (ilOffset)
        {
            Contract.Requires(target != null);
            Contract.Requires(value != null);
            Contract.Requires(target.GetExpressionType().IsByRef);
            this.Target = target;
            this.Value = value;
        }

        public Expression Target { get; internal set; }

        public Expression Value { get; internal set; }

        public override Type GetExpressionType()
        {
            return Target.GetExpressionType().GetElementType();
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("(*{0}) = {1}", Target.ToString(), Value.ToString());
        }
    }
}
