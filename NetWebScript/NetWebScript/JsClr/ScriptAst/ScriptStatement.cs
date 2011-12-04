using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public abstract class ScriptStatement
    {
        /// <summary>
        /// Does statement requires a ';' in textual representation
        /// </summary>
        protected virtual bool IsStandalone
        {
            get { return false; }
        }

        protected static void Append(StringBuilder builder, List<ScriptStatement> statements)
        {
            foreach (ScriptStatement statement in statements)
            {
                builder.Append("\t");
                builder.Append(statement.ToString().Replace("\n","\n\t"));
                if (!statement.IsStandalone)
                {
                    builder.Append(';');
                }
                builder.AppendLine();
            }
        }

        public abstract T Accept<T>(IScriptStatementVisitor<T> visitor);

        public abstract void Accept(IScriptStatementVisitor visitor);

        //public virtual bool IsSame(Statement statement)
        //{
        //    return this == statement;
        //}
    }
}
