using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public sealed class LiteralExpression : Expression
    {
        private Type type;

        public LiteralExpression(object value)
            : base(null)
        {
            this.Value = value;
            if (value != null)
            {
                type = value.GetType();
            }
        }

        public LiteralExpression(int ilOffset, object value)
            : base(ilOffset)
        {
            this.Value = value;
            if (value != null)
            {
                type = value.GetType();
                if (type == typeof(int))
                {
                    int intValue = (int)value;
                    if (intValue == 0 || intValue == 1)
                    {
                        // Only 0 (false) and 1 (true) are ambigous
                        type = null;
                    }
                }
            }
        }

        public object Value { get; private set; }

        public override Type GetExpressionType()
        {
            return type;
        }

        public void SetExpressionType(Type type)
        {
            this.type = type;
            if (Value != null && Value.GetType() != type)
            {
                if (type == typeof(bool))
                {
                    Value = ((int)Value == 0) ? false : true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
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

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override bool IsConstInMethod()
        {
            return true;
        }
    }
}
