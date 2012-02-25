using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Boolean))]
    internal sealed class BooleanEquiv
    {
        private readonly bool value;

        public BooleanEquiv ( bool value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value ? "True" : "False";
        }

        public override bool Equals(object obj)
        {
            var other = obj as BooleanEquiv;
            return other != null && this.value == other.value;
        }

        public override int GetHashCode()
        {
            return value ? 1 : 0;
        }

        public bool Value { get { return value; } } 

        public static bool TryParse(string value, out bool result)
        {
            if (value != null)
            {
                value = value.Trim().ToLowerInvariant();
                if (value == "true")
                {
                    result = true;
                    return true;
                }
                if (value == "false")
                {
                    result = false;
                    return true;
                }
            }
            result = false;
            return false;
        }

        public static bool Parse(string value)
        {
            bool result;
            if (!TryParse(value, out result))
            {
                throw new System.Exception("FormatException");
            }
            return result;
        }
    }
}
