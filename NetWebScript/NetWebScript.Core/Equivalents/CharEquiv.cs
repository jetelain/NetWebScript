using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Char))]
    internal sealed class CharEquiv
    {
        private readonly int value;

        public CharEquiv ( int value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return JSString.FromCharCode(value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CharEquiv;
            return other != null && this.value == other.value;
        }

        public override int GetHashCode()
        {
            return value;
        }

        public int Value { get { return value; } } 

    }
}
