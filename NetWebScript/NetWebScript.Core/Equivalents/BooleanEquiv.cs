using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Boolean))]
    public sealed class BooleanEquiv
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

    }
}
