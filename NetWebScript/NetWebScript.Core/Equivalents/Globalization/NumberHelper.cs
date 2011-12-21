using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Equivalents.Globalization
{
    internal static class NumberFormat
    {
        public static string Format(JSNumber value, string format, NumberFormatInfo info)
        {
            if (!JSNumber.IsFinite(value))
            {
                if (value == JSNumber.POSITIVE_INFINITY)
                {
                    return info.PositiveInfinitySymbol;
                }
                if (value == JSNumber.NEGATIVE_INFINITY)
                {
                    return info.NegativeInfinitySymbol;
                }
                return info.NaNSymbol;
            }
            if (format == "null" || format == "g" || format == "G")
            {
                if (info.NumberDecimalSeparator != ".")
                {
                    return value.ToString().Replace(".", info.NumberDecimalSeparator);
                }
                return value.ToString();
            }

            throw new System.NotImplementedException();
        }
    }
}
