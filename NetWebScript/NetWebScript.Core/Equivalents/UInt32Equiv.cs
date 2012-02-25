using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.UInt32))]
    internal sealed class UInt32Equiv
    {
        private readonly JSNumber value;

        public UInt32Equiv ( JSNumber value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public JSNumber Value { get { return value; } } 

    }
}
