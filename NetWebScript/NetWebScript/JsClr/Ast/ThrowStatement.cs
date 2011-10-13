using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ThrowStatement : Statement
    {
        public ThrowStatement(Expression value)
        {
            this.Value = value;
        }

        public Expression Value { get; internal set; }

        public override string ToString()
        {
            return String.Format("throw {0}", Value.ToString());
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
