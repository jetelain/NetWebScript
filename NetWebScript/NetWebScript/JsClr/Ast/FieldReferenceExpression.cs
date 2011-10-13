using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{
    public sealed class FieldReferenceExpression : AssignableExpression
    {
        public FieldReferenceExpression(int? ilOffset, Expression target, FieldInfo field)
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
            return Field.FieldType;
        }

        public override string ToString()
        {
            if (Field.IsStatic)
            {
                return String.Format("{0}.{1}", Field.DeclaringType.Name, Field.Name);
            }
            return String.Format("{0}.{1}", Target.ToString(), Field.Name);
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override int? FirstIlOffset
        {
            get
            {
                if (Target != null)
                {
                    return Target.IlOffset;
                }
                return IlOffset;
            }
        }

        public override Expression Clone()
        {
            if (Target != null)
            {
                return new FieldReferenceExpression(IlOffset, null, Field);
            }
            return new FieldReferenceExpression(IlOffset, Target.Clone(), Field);
        }
    }
}
