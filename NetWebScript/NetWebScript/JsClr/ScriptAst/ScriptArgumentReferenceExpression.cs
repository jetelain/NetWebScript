using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptArgumentReferenceExpression : ScriptAssignableExpression
    {
        public ScriptArgumentReferenceExpression(ScriptArgument arg)
            : this(null, arg)
        {
            Contract.Requires(arg != null);
        }

        public ScriptArgumentReferenceExpression(int? ilOffset, ScriptArgument arg)
            : base(ilOffset)
        {
            Contract.Requires(arg != null);
            this.Argument = arg;
        }

        public ScriptArgument Argument { get; internal set; }

        public override string ToString()
        {
            return Argument.Name;
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override ScriptExpression Clone()
        {
            return new ScriptArgumentReferenceExpression(IlOffset, Argument);
        }

        public override bool HasSideEffect()
        {
            // Evaluating an argument has no side effect
            return false;
        }
    }
}
