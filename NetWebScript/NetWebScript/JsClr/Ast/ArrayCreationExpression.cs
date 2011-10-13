using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ArrayCreationExpression : Expression
    {
        public ArrayCreationExpression(int? ilOffset, Type type, Expression size)
            : base(ilOffset)
        {
            this.ItemType = type;
            this.Type = type.MakeArrayType();
            this.Size = size;
        }

        public Type ItemType { get; internal set; }

        public Type Type { get; internal set; }

        public Expression Size { get; internal set; }

        public List<Expression> Initialize { get; set; }

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
                foreach (Expression arg in Initialize)
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

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override int? FirstIlOffset
        {
            get
            {
                return Size.FirstIlOffset;
            }
        }
    }
}
