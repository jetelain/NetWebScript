using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.Ast
{
    public sealed class MakeByRefArgumentExpression : Expression
    {
        internal MakeByRefArgumentExpression(int ilOffset, ParameterInfo arg)
            : base(ilOffset)
        {
            Contract.Requires(arg != null);
            this.Argument = arg;
        }

        public ParameterInfo Argument { get; internal set; }

        public override Type GetExpressionType()
        {
            return Argument.ParameterType.MakeByRefType();
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
            return String.Format("&({0})", Argument.Name);
        }


        public override Expression GetRefValue()
        {
            return new ArgumentReferenceExpression(IlOffset, Argument);
        }
    }
}
