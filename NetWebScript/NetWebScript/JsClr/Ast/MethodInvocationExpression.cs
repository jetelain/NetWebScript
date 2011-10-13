using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class MethodInvocationExpression : Expression
    {
        public MethodInvocationExpression(int? ilOffset, bool vir, MethodBase method, Expression target, List<Expression> arguments)
            : base(ilOffset)
        {
            Contract.Requires(method != null);
            this.Virtual = vir;
            this.Method = method;
            this.Target = target;
            this.Arguments = arguments;
        }

        public bool Virtual { get; internal set; }

        public MethodBase Method { get; internal set; }

        public Expression Target { get; internal set; }

        public List<Expression> Arguments { get; internal set; }

        public override Type GetExpressionType()
        {
            MethodInfo fi = Method as MethodInfo;
            if (fi != null)
            {
                return fi.ReturnType;
            }
            return typeof(void);
        }

        internal bool IsExplicit
        {
            get { return !Virtual || !Method.IsVirtual; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (Target != null)
            {
                builder.Append(Target.ToString());
            }
            else
            {
                builder.Append(Method.DeclaringType.Name);
            }
            builder.Append('.');
            builder.Append(Method.Name);
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
                if (Target != null)
                {
                    return Target.FirstIlOffset;
                }
                if (Arguments != null && Arguments.Count > 0)
                {
                    return Arguments[0].FirstIlOffset;
                }
                return IlOffset;
            }
        }
    }
}
