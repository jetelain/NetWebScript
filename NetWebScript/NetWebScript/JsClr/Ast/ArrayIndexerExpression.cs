using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ArrayIndexerExpression : AssignableExpression
    {
        public ArrayIndexerExpression(int? ilOffset, Expression target, Expression index) :
            base(ilOffset)
        {
            Contract.Requires(target != null);
            Contract.Requires(index != null);
            this.Target = target;
            this.Index = index;
        }

        public Expression Target { get; internal set; }

        public Expression Index { get; internal set; }

        public override Type GetExpressionType()
        {
            Type targetType = Target.GetExpressionType();
            if (targetType != null && targetType.IsArray)
            {
                return targetType.GetElementType();
            }
            return null;
        }

        public override string ToString()
        {
            return String.Format("{0}[{1}]", Target.ToString(), Index.ToString());
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
                return Index.FirstIlOffset;
            }
        }

    }
}
