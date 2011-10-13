using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ThisReferenceExpression : Expression
    {
        private readonly Type type;

        public ThisReferenceExpression(int? ilOffset, Type type)
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

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override bool IsConstInMethod()
        {
            return true;
        }

        public override Expression Clone()
        {
            return new ThisReferenceExpression(IlOffset, type);
        }
    }
}
