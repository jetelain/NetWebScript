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
    [ScriptEquivalent(typeof(System.Double))]
    public sealed class DoubleEquiv : IFormattable
    {
        private readonly JSNumber value;

        public DoubleEquiv ( JSNumber value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return NumberFormat.FormatFloat(value, null, NumberFormatInfo.CurrentInfo);
        }

        public override bool Equals(object obj)
        {
            var other = obj as DoubleEquiv;
            return other != null && this.value == other.value;
        }

        public override int GetHashCode()
        {
            return (int)(((double)value) % 0x7fffffff);
        }

        public JSNumber Value { get { return value; } }

        public string ToString(string format, IFormatProvider provider)
        {
            return NumberFormat.FormatFloat(value, format, NumberFormatInfo.GetInstance(provider));
        }

        public string ToString(IFormatProvider provider)
        {
            return NumberFormat.FormatFloat(value, null, NumberFormatInfo.GetInstance(provider));
        }

        public static double Parse(string s, IFormatProvider provider)
        {
            return NumberFormat.ParseFloat(s, NumberFormatInfo.GetInstance(provider));
        }
    }
}
