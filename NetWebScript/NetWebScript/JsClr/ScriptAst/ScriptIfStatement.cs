using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptIfStatement : ScriptStatement
    {

        public ScriptIfStatement ( ScriptExpression  condition, List<ScriptStatement> @then, List<ScriptStatement> @else )
        {
            Condition = condition;
            Then = @then;
            Else = @else;
        }

        public ScriptExpression Condition { get; internal set; }
        
        public List<ScriptStatement> Then { get; internal set; }

        public List<ScriptStatement> Else { get; internal set; }

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
