using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptArrayIndexerExpression : ScriptAssignableExpression
    {
        public ScriptArrayIndexerExpression(int? ilOffset, ScriptExpression target, ScriptExpression index) :
            base(ilOffset)
        {
            Contract.Requires(target != null);
            Contract.Requires(index != null);
            this.Target = target;
            this.Index = index;
        }

        public ScriptExpression Target { get; internal set; }

        public ScriptExpression Index { get; internal set; }

        public override Type GetExpressionType()
        {
            Type targetType = Target.GetExpressionType();
            if (targetType != null && targetType.IsArray)
            {
                return targetType.GetElementType();
            }
            return null;
        }

        public override string ToString()
        {
            return String.Format("{0}[{1}]", Target.ToString(), Index.ToString());
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
