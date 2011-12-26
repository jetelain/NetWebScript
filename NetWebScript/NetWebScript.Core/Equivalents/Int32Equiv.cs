using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using NetWebScript.Equivalents.Globalization;
using System.Globalization;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Int32))]
    public sealed class Int32Equiv : IFormattable
    {
        private readonly JSNumber value;

        public Int32Equiv ( JSNumber value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Int32Equiv;
            return other != null && this.value == other.value;
        }

        public override int GetHashCode()
        {
            return (int)value;
        }

        public JSNumber Value { get { return value; } } 

        public string ToString(string format, IFormatProvider provider)
        {
            return NumberFormat.FormatInteger(value, format, (NumberFormatInfo)provider.GetFormat(typeof(NumberFormatInfo)));
        }

        public static int Parse(string s)
        {
            return JSNumber.ParseInt(s);
        }
    }
}
