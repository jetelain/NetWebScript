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
        public ScriptFieldReferenceExpression(ScriptExpression target, IInvocableField field)
            : this(null, target, field)
        {

        }

        public ScriptFieldReferenceExpression(int? ilOffset, ScriptExpression target, IInvocableField field)
            : base(ilOffset)
        {
            Contract.Requires(field != null);
            Contract.Requires(field.IsStatic ? target == null : target != null);
            this.Target = target;
            this.Field = field;
        }

        public ScriptExpression Target { get; internal set; }

        public IInvocableField Field { get; internal set; }

        public override string ToString()
        {
            if (Field.IsStatic)
            {
                return String.Format("{0}.{1}", Field.DeclaringType.DisplayName, Field.DisplayName);
            }
            return String.Format("{0}.{1}", Target.ToString(), Field.DisplayName);
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
