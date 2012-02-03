using System;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptLiteralExpression : ScriptExpression
    {
        private readonly IScriptType type;

        public static readonly ScriptLiteralExpression NullValue = new ScriptLiteralExpression(null, null, null);

        public ScriptLiteralExpression(int? ilOffset, object value, IScriptType type)
            : base(ilOffset)
        {
            Contract.Requires(value == null || (type != null && type.Serializer != null));
            this.Value = value;
            this.type = type;
        }

        public object Value { get; private set; }

        public IScriptType Type { get { return type; } }

        public override Type GetExpressionType()
        {
            if (type == null)
            {
                return null;
            }
            return type.Type;
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return "null";
            }
            
            if (Value is String)
            {
                return String.Format("\"{0}\"", Value);
            }
            return Value.ToString();
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
