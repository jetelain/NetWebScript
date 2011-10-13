using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class IfStatement : Statement
    {
        public Expression Condition { get; internal set; }
        
        public List<Statement> Then { get; internal set; }

        public List<Statement> Else { get; internal set; }

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
            builder.AppendFormat("if ( {0} )", Condition.ToString());
            builder.AppendLine();
            builder.AppendLine("{");
            Append(builder, Then);
            builder.Append("}");
            if (Else != null)
            {
                builder.AppendLine();
                builder.AppendLine("else");
                builder.AppendLine("{");
                Append(builder, Else);
                builder.Append("}");
            }
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
