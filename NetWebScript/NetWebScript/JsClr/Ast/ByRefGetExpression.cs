using System;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ByRefGetExpression : Expression
    {
        internal ByRefGetExpression(int ilOffset, Expression target)
            : base (ilOffset)
        {
            Contract.Requires(target != null);
            Contract.Requires(target.GetExpressionType().IsByRef);
            this.Target = target;
        }

        public Expression Target { get; internal set; }

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
            return String.Format("(*{0})", Target.ToString());
        }
    }
}
