using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptReturnStatement : ScriptStatement
    {
        public ScriptReturnStatement ( ScriptExpression value)
        {
            this.Value = value;
        }

        public ScriptExpression Value { get; set; }

        public override string ToString()
        {
            if (Value != null)
            {
                return String.Format("return {0}", Value.ToString());
            }
            return "return";
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
