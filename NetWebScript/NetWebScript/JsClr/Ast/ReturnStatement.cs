using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ReturnStatement : Statement
    {
        public Expression Value { get; set; }

        public override string ToString()
        {
            if (Value != null)
            {
                return String.Format("return {0}", Value.ToString());
            }
            return "return";
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
