using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.Ast
{
    public sealed class DebugPointExpression : Expression
    {
        internal DebugPointExpression(PdbSequencePoint point)
            : base(point.Offset)
        {
            this.Point = point;
        }

        internal DebugPointExpression(PdbSequencePoint point, Expression expression)
            : base(point.Offset)
        {
            this.Point = point;
            this.Value = expression;
        }

        public Expression Value { get; internal set; }

        internal PdbSequencePoint Point { get; private set; }

        public override Type GetExpressionType()
        {
            if (Value != null)
            {
                return Value.GetExpressionType();
            }
            return typeof(void);
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
            if (Value != null)
            {
                return String.Format("/*{0}*/{1}", Point.ToString(), Value.ToString());
            }
            return String.Format("/*{0}*/",Point.ToString());
        }

        public override Expression Negate()
        {
            return new DebugPointExpression(Point, Value.Negate());
        }
    }
}
