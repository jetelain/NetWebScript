using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ObjectCreationExpression : Expression
    {

        public ObjectCreationExpression(int? ilOffset,ConstructorInfo constructor, List<Expression> arguments)
            : base(ilOffset)
        {
            Contract.Requires(constructor != null);
            this.Constructor = constructor;
            this.Arguments = arguments;
        }

        public ConstructorInfo Constructor { get; internal set; }

        public List<Expression> Arguments { get; internal set; }

        public override Type GetExpressionType()
        {
            return Constructor.DeclaringType;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("new ");
            builder.Append(Constructor.DeclaringType.Name);
            builder.Append('(');
            bool first = true;
            foreach (Expression arg in Arguments)
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
            builder.Append(')');
            return builder.ToString();
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
                if (Arguments != null && Arguments.Count > 0)
                {
                    return Arguments[0].FirstIlOffset;
                }
                return IlOffset;
            }
        }
    }
}
