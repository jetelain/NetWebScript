using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptSwitchStatement : ScriptStatement
    {
        public ScriptSwitchStatement(ScriptExpression value, List<ScriptCase> cases)
        {
            Value = value;
            Cases = cases;
        }

        public ScriptExpression Value { get; internal set; }

        public List<ScriptCase> Cases { get; private set; }

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
            foreach (ScriptCase @case in Cases)
            {
                builder.AppendLine();
                if (@case.Value == ScriptCase.DefaultCase)
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

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
