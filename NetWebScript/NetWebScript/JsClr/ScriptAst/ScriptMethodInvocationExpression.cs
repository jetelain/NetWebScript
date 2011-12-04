using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptMethodInvocationExpression : ScriptExpression
    {
        public ScriptMethodInvocationExpression(int? ilOffset, bool vir, IScriptMethodBase method, ScriptExpression target, List<ScriptExpression> arguments)
            : base(ilOffset)
        {
            Contract.Requires(method != null);
            this.Virtual = vir;
            this.Method = method;
            this.Target = target;
            this.Arguments = arguments;
        }

        public bool Virtual { get; internal set; }

        public IScriptMethodBase Method { get; internal set; }

        public ScriptExpression Target { get; internal set; }

        public List<ScriptExpression> Arguments { get; internal set; }

        public override Type GetExpressionType()
        {
            IScriptMethod fi = Method as IScriptMethod;
            if (fi != null)
            {
                return ((MethodInfo)fi.Method).ReturnType;
            }
            return typeof(void);
        }

        internal bool IsExplicit
        {
            get { return !Virtual || !Method.Method.IsVirtual; }
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
                builder.Append(Method.Owner.Type.Name);
            }
            builder.Append('.');
            builder.Append(Method.Method.Name);
            builder.Append('(');
            bool first = true;
            foreach (ScriptExpression arg in Arguments)
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
