using System;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.TypeSystem.Serializers;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptLiteralExpression : ScriptExpression
    {
        private readonly IValueSerializer serializer;

        public static readonly ScriptLiteralExpression NullValue = new ScriptLiteralExpression(null, null, null);

        public static ScriptLiteralExpression StringLiteral(string value)
        {
            return new ScriptLiteralExpression(value, StringSerializer.Instance);
        }

        public static ScriptLiteralExpression IntegerLiteral(int value)
        {
            return new ScriptLiteralExpression(value, NumberSerializer.Instance);
        }

        public ScriptLiteralExpression(object value, IValueSerializer serializer)
            : this(null, value, serializer)
        {

        }

        public ScriptLiteralExpression(int? ilOffset, object value, IValueSerializer serializer)
            : base(ilOffset)
        {
            Contract.Requires(value == null || serializer != null);
            this.Value = value;
            this.serializer = serializer;
        }

        public object Value { get; private set; }

        public IValueSerializer Serializer { get { return serializer; } }

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
