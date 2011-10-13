using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class WhileStatement : Statement
    {
        public Expression Condition { get; internal set; }

        public List<Statement> Body { get; internal set; }

        protected override bool IsStandalone
        {
            get
            {
                return true;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("while ( {0} )", Condition.ToString());
            builder.AppendLine();
            builder.AppendLine("{");
            Append(builder, Body);
            builder.Append("}");
            return builder.ToString();
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
