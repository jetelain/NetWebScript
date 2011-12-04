using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptFieldReferenceExpression : ScriptAssignableExpression
    {
        public ScriptFieldReferenceExpression(int? ilOffset, ScriptExpression target, IScriptField field)
            : base(ilOffset)
        {
            Contract.Requires(field != null);
            this.Target = target;
            this.Field = field;
        }

        public ScriptExpression Target { get; internal set; }

        public IScriptField Field { get; internal set; }


        public override Type GetExpressionType()
        {
            return Field.Field.FieldType;
        }

        public override string ToString()
        {
            if (Field.Field.IsStatic)
            {
                return String.Format("{0}.{1}", Field.Owner.Type.Name, Field.Field.Name);
            }
            return String.Format("{0}.{1}", Target.ToString(), Field.Field.Name);
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override ScriptExpression Clone()
        {
            if (Target != null)
            {
                return new ScriptFieldReferenceExpression(IlOffset, null, Field);
            }
            return new ScriptFieldReferenceExpression(IlOffset, Target.Clone(), Field);
        }
    }
}
