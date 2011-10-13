using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.Ast
{
    public sealed class VariableReferenceExpression : AssignableExpression
    {
        public VariableReferenceExpression(int? ilOffset, LocalVariable variable)
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
            return new VariableReferenceExpression(IlOffset, Variable);
        }
    }
}
