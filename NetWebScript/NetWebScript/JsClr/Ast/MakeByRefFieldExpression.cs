using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace NetWebScript.JsClr.Ast
{
    public sealed class MakeByRefFieldExpression : Expression
    {
        public MakeByRefFieldExpression(int ilOffset, Expression target, FieldInfo field)
            : base(ilOffset)
        {
            Contract.Requires(field != null);
            this.Target = target;
            this.Field = field;
        }

        public Expression Target { get; internal set; }

        public FieldInfo Field { get; internal set; }

        public override Type GetExpressionType()
        {
            return Field.FieldType.MakeByRefType();
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
            if (Field.IsStatic)
            {
                return String.Format("&({0}.{1})", Field.DeclaringType.Name, Field.Name);
            }
            return String.Format("&({0}.{1})", Target.ToString(), Field.Name);
        }

    }
}
