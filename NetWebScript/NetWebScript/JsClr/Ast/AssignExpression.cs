using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class AssignExpression : Expression
    {
        public AssignExpression(int? ilOffset, AssignableExpression target, Expression value)
            : base (ilOffset)
        {
            this.Target = target;
            this.Value = value;
        }

        public AssignableExpression Target { get; internal set; }

        public Expression Value { get; internal set; }

        public override Type GetExpressionType()
        {
            return Target.GetExpressionType();
        }

        public override string ToString()
        {
            return String.Format("{0} = {1}", Target.ToString(), Value.ToString());
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        //public override bool IsAssignementOrSame(Expression b)
        //{
        //    if (this == b)
        //    {
        //        return true;
        //    }
        //    return Value.IsAssignementOrSame(b);
        //}

        public override int? FirstIlOffset
        {
            get
            {
                if (Target.FirstIlOffset == IlOffset || Target.FirstIlOffset == null)
                {
                    return Value.FirstIlOffset;
                }
                return Target.FirstIlOffset;
            }
        }
    }
}
