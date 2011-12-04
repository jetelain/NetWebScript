using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptTryCatchStatement : ScriptStatement
    {
        public ScriptTryCatchStatement(List<ScriptStatement> body, List<ScriptStatement> @finally, List<ScriptStatement> @catch)
        {
            this.Body = body;
            this.Finally = @finally;
            this.Catch = @catch;
        }


        public List<ScriptStatement> Body { get; internal set; }
        public List<ScriptStatement> Finally { get; internal set; }
        public List<ScriptStatement> Catch { get; internal set; }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override bool IsStandalone
        {
            get
            {
                return true;
            }
        }

        public bool HasCatch
        {
            get { return Catch != null && Catch.Count > 0; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("try");
            builder.AppendLine("{");
            Append(builder, Body);
            builder.Append("}");

            if (Catch != null)
            {
                builder.AppendLine();
                builder.Append("catch ($exception)");
                builder.AppendLine("{");
                Append(builder, Catch);
                builder.Append("}");
            }

            if (Finally != null)
            {
                builder.AppendLine();
                builder.AppendLine("finally");
                builder.AppendLine("{");
                Append(builder, Finally);
                builder.Append("}");
            }
            return builder.ToString();
        }
    }
}
