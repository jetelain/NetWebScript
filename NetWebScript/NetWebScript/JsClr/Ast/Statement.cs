using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.Ast
{
    public abstract class Statement
    {
        /// <summary>
        /// Does statement requires a ';' in textual representation
        /// </summary>
        protected virtual bool IsStandalone
        {
            get { return false; }
        }

        protected static void Append(StringBuilder builder, List<Statement> statements)
        {
            foreach (Statement statement in statements)
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

        public abstract T Accept<T>(IStatementVisitor<T> visitor);

        public abstract void Accept(IStatementVisitor visitor);

        //public virtual bool IsSame(Statement statement)
        //{
        //    return this == statement;
        //}
    }
}
