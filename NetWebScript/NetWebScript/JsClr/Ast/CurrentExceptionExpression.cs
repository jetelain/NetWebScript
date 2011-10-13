using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class CurrentExceptionExpression : Expression
    {
        private readonly Type type;

        internal CurrentExceptionExpression(Type type)
            : base(null)
        {
            Contract.Requires(type != null);
            this.type = type;
        }

        public override Type GetExpressionType()
        {
            return type;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IStatementVisitor visitor)
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

        public override Expression Clone()
        {
            return new CurrentExceptionExpression(type);
        }
    }
}
