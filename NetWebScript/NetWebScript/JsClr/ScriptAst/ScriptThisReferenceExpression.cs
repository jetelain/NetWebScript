using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptThisReferenceExpression : ScriptExpression
    {
        private readonly Type type;

        public ScriptThisReferenceExpression(int? ilOffset, Type type)
            : base(ilOffset)
        {
            Contract.Requires(type != null);
            Contract.Requires(type != typeof(void));
            this.type = type;
        }

        public override Type GetExpressionType()
        {
            return type;
        }

        public override string ToString()
        {
            return "this";
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override bool IsConstInMethod()
        {
            return true;
        }

        public override ScriptExpression Clone()
        {
            return new ScriptThisReferenceExpression(IlOffset, type);
        }
    }
}
