using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class SwitchStatement : Statement
    {
        public SwitchStatement()
        {
            Cases = new List<Case>();
        }

        public Expression Value { get; internal set; }

        public List<Case> Cases { get; private set; }

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
            builder.AppendFormat("switch ( {0} )", Value.ToString());
            foreach ( Case @case in Cases )
            {
                builder.AppendLine();
                if (@case.Value == Case.DefaultCase)
                {
                    builder.AppendFormat("\tdefault:");
                }
                else
                {
                    builder.AppendFormat("\tcase {0}:", @case.Value);
                }
                builder.AppendLine();
                Append(builder, @case.Statements);
                builder.Append("\tbreak;");
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
