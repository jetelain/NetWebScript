using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.NumberFormatInfo))]
    public class NumberFormatInfoEquiv : IFormatProvider
    {
        public string NaNSymbol { get; set; }
        public string NegativeInfinitySymbol { get; set; }
        public string PositiveInfinitySymbol { get; set; }

        public string NumberDecimalSeparator { get; set; }
        public string NegativeSign { get; set; }
        public object GetFormat(Type formatType)
        {
            if (object.Equals(formatType,typeof(System.Globalization.NumberFormatInfo)))
            {
                return this;
            }
            return null;
        }
    }
}
