﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptObjectCreationExpression : ScriptExpression
    {
        public ScriptObjectCreationExpression(IInvocableConstructor constructor, List<ScriptExpression> arguments)
            : this(null, constructor, arguments)
        {
        }

        public ScriptObjectCreationExpression(int? ilOffset, IInvocableConstructor constructor, List<ScriptExpression> arguments)
            : base(ilOffset)
        {
            Contract.Requires(constructor != null);
            this.Constructor = constructor;
            this.Arguments = arguments;
        }

        public IInvocableConstructor Constructor { get; internal set; }

        public List<ScriptExpression> Arguments { get; internal set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("new ");
            builder.Append(Constructor.Owner.DisplayName);
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
