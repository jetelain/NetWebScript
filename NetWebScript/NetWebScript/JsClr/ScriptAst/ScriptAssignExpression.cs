using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptAssignExpression : ScriptExpression
    {
        public ScriptAssignExpression(ScriptAssignableExpression target, ScriptExpression value)
            : this(null, target, value)
        {
        }

        public ScriptAssignExpression(int? ilOffset, ScriptAssignableExpression target, ScriptExpression value)
            : base (ilOffset)
        {
            this.Target = target;
            this.Value = value;
        }

        public ScriptAssignableExpression Target { get; internal set; }

        public ScriptExpression Value { get; internal set; }

        public override string ToString()
        {
            return String.Format("{0} = {1}", Target.ToString(), Value.ToString());
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
