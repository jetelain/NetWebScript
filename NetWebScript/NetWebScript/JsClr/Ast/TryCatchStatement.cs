using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class TryCatchStatement : Statement
    {
        public List<Statement> Body { get; internal set; }
        public List<Statement> Finally { get; internal set; }
        public List<Catch> CatchList { get; internal set; }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IStatementVisitor visitor)
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
            get { return CatchList != null && CatchList.Count > 0; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("try");
            builder.AppendLine("{");
            Append(builder, Body);
            builder.Append("}");

            if (CatchList != null)
            {
                foreach (Catch @catch in CatchList)
                {
                    builder.AppendLine();
                    builder.Append("catch (");
                    builder.Append(@catch.Type.FullName);
                    builder.AppendLine(")");
                    builder.AppendLine("{");
                    Append(builder, @catch.Body);
                    builder.Append("}");
                }
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
