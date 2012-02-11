using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptArrayCreationExpression : ScriptExpression
    {
        public ScriptArrayCreationExpression(ScriptExpression size)
            : this(null, size)
        {

        }

        public ScriptArrayCreationExpression(int? ilOffset, ScriptExpression size)
            : base(ilOffset)
        {
            Contract.Requires(size != null);
            this.Size = size;
        }

        public ScriptExpression Size { get; internal set; }

        public List<ScriptExpression> Initialize { get; set; }

        public override string ToString()
        {
            if (Initialize != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("new [{0}] {{", Size.ToString());
                bool first = true;
                foreach (ScriptExpression arg in Initialize)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(',');
                    }
                    builder.Append(arg.ToString());
                }
                builder.Append("}");
                return builder.ToString();
            }


            return String.Format("new [{0}]", Size.ToString());
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
