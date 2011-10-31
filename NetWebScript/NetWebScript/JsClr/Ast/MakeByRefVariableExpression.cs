using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.Ast
{
    public sealed class MakeByRefVariableExpression : Expression
    {
        internal MakeByRefVariableExpression(int ilOffset, LocalVariable variable)
            : base(ilOffset)
        {
            Contract.Requires(variable != null);
            this.Variable = variable;
        }

        public LocalVariable Variable { get; internal set; }

        public override Type GetExpressionType()
        {
            return Variable.LocalType.MakeByRefType();
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
            return String.Format("&({0})", Variable.Name);
        }


        public override Expression GetRefValue()
        {
            return new VariableReferenceExpression(IlOffset, Variable);
        }
    }
}
