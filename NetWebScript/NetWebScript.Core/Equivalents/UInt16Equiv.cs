using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.UInt16))]
    internal sealed class UInt16Equiv
    {
        private readonly JSNumber value;

        public UInt16Equiv ( JSNumber value )
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
