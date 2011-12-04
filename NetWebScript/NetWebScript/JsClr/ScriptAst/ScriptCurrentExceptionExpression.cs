using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptCurrentExceptionExpression : ScriptExpression
    {
        private readonly Type type;

        internal ScriptCurrentExceptionExpression(Type type)
            : base(null)
        {
            Contract.Requires(type != null);
            this.type = type;
        }

        public override Type GetExpressionType()
        {
            return type;
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "$exception";
        }

        public override bool IsConstInMethod()
        {
            return true;
        }

        public override ScriptExpression Clone()
        {
            return new ScriptCurrentExceptionExpression(type);
        }
    }
}
