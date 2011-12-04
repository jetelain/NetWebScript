using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptVariableReferenceExpression : ScriptAssignableExpression
    {
        public ScriptVariableReferenceExpression(int? ilOffset, LocalVariable variable)
            : base(ilOffset)
        {
            this.Variable = variable;
        }

        public LocalVariable Variable { get; internal set; }

        public override Type GetExpressionType()
        {
            return Variable.LocalType;
        }

        public override string ToString()
        {
            return Variable.Name;
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
            return new ScriptVariableReferenceExpression(IlOffset, Variable);
        }

        public override bool HasSideEffect()
        {
            // Evaluating a variable has no side effect
            return false;
        }
    }
}
