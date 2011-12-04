using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptArrayCreationExpression : ScriptExpression
    {
        public ScriptArrayCreationExpression(int? ilOffset, Type type, ScriptExpression size)
            : base(ilOffset)
        {
            this.ItemType = type;
            this.Type = type.MakeArrayType();
            this.Size = size;
        }

        public Type ItemType { get; internal set; }

        public Type Type { get; internal set; }

        public ScriptExpression Size { get; internal set; }

        public List<ScriptExpression> Initialize { get; set; }

        public override Type GetExpressionType()
        {
            return Type;
        }

        public override string ToString()
        {
            if (Initialize != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("new {0}[{1}] {{", ItemType.Name, Size.ToString());
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


            return String.Format("new {0}[{1}]", ItemType.Name, Size.ToString());
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
