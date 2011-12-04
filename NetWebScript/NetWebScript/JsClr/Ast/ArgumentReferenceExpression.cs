using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class ArgumentReferenceExpression : AssignableExpression
    {
        public ArgumentReferenceExpression(int? ilOffset, ParameterInfo arg)
            : base(ilOffset)
        {
            Contract.Requires(arg != null);
            this.Argument = arg;
        }

        public ParameterInfo Argument { get; internal set; }

        public override Type GetExpressionType()
        {
            return Argument.ParameterType;
        }

        public override string ToString()
        {
            return Argument.Name;
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Clone()
        {
            return new ArgumentReferenceExpression(IlOffset, Argument);
        }

        public override bool HasSideEffect()
        {
            // Evaluating an argument has no side effect
            return false;
        }
    }
}
