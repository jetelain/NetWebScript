using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Int16))]
    internal sealed class Int16Equiv
    {
        private readonly JSNumber value;

        public Int16Equiv ( JSNumber value )
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
