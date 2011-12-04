using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptUnaryExpression : ScriptExpression
    {
        public ScriptUnaryExpression(int? ilOffset, ScriptUnaryOperator @operator, ScriptExpression operand)
            : base (ilOffset)
        {
            Contract.Requires(operand != null);
            this.Operator = @operator;
            this.Operand = operand;
        }

        public ScriptUnaryOperator Operator { get; internal set; }

        public ScriptExpression Operand { get; internal set; }

        public override ScriptExpression Negate()
        {
            if (Operator == ScriptUnaryOperator.LogicalNot)
            {
                return Operand;
            }
            return base.Negate();
        }

        public override Type GetExpressionType()
        {
            if (Operator == ScriptUnaryOperator.LogicalNot)
            {
                return typeof(bool);
            }
            return Operand.GetExpressionType();
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", Operator, Operand.ToString() );
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
