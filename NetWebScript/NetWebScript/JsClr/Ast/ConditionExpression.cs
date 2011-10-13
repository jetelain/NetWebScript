using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ConditionExpression : Expression
    {

        public ConditionExpression(int? ilOffset, Expression cond, Expression @then, Expression @else)
            : base(ilOffset)
        {
            this.Condition = cond;
            this.Then = @then;
            this.Else = @else;
        }


        public Expression Condition { get; internal set; }
        public Expression Then { get; internal set; }
        public Expression Else { get; internal set; }

        public override Type GetExpressionType()
        {
            Type a = Then.GetExpressionType();
            Type b = Else.GetExpressionType();
            if (a == null && b == null)
            {
                return null;
            }
            if (a == null)
            {
                return b;
            }
            if (b == null)
            {
                return a;
            }
            if (a == b || a.IsAssignableFrom(b))
            {
                return a;
            }
            return b;
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("({0}?{1}:{2})", Condition.ToString(),Then.ToString(),Else.ToString());
        }

        public override int? FirstIlOffset
        {
            get
            {
                return Condition.FirstIlOffset;
            }
        }
    }
}
