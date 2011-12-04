using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptDoWhileStatement : ScriptStatement
    {
        public ScriptDoWhileStatement(ScriptExpression condition, List<ScriptStatement> body)
        {
            Condition = condition;
            Body = body;
        }

        public ScriptExpression Condition { get; internal set; }

        public List<ScriptStatement> Body { get; internal set; }

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
            builder.AppendLine("do");
            builder.AppendLine("{");
            Append(builder, Body);
            builder.AppendLine("}");
            builder.AppendFormat("while ( {0} )", Condition.ToString());
            
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
