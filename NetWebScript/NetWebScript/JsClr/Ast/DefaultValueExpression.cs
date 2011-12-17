using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class DefaultValueExpression : Expression
    {
        private readonly Type type;

        public DefaultValueExpression ( int offset, Type type )
            : base(offset)
        {
            Contract.Requires(type != null);
            this.type = type;
        }

        public override Type GetExpressionType()
        {
            return type;
        }
        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override bool HasSideEffect()
        {
            return false;
        }

        public override bool IsConstInMethod()
        {
            return true;
        }

        public Type Type { get { return type; } }

    }
}
