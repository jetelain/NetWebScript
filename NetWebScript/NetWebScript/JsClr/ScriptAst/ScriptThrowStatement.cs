using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptThrowStatement : ScriptStatement
    {
        public ScriptThrowStatement(ScriptExpression value)
        {
            this.Value = value;
        }

        public ScriptExpression Value { get; internal set; }

        public override string ToString()
        {
            return String.Format("throw {0}", Value.ToString());
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
