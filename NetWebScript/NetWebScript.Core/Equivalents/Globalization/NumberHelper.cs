using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    internal static class NumberFormat
    {
        public static string FormatInteger(JSNumber value, JSString format, NumberFormatInfo info)
        {
            if (format == null )
            {
                if (info.NegativeSign == "-")
                {
                    return value.ToString();
                }
                format = "d";
            }

            char formatChar = (char)format.CharCodeAt(0);

            int precision = -1;
            if (format.Length > 1)
            {
                precision = JSNumber.ParseInt(format.Substr(1));
            }
            string str;

            switch(formatChar)
            {
                case 'x':
                case 'X':
                    if (value < 0)
                    {
                        str = ((JSNumber)((value >> 16) & 0xffff)).ToString(16);
                        str += ((JSNumber)(value & 0xffff)).ToString(16);
                    }
                    else
                    {
                        str = ((JSNumber)value).ToString(16);
                    }
                    if (formatChar == 'X')
                    {
                        str = str.ToUpperInvariant();
                    }
                    if (str.Length < precision)
                    {
                        str = str.PadLeft(precision, '0');
                    } 
                    break;
                case 'd':
                case 'D':
                    str = ((JSNumber)JSMath.Abs(value)).ToString();
                    if (str.Length < precision) 
                    {
                        str = str.PadLeft(precision, '0');
                    }
                    if (value < 0) 
                    {
                        str = info.NegativeSign + str;
                    }
                    break;
                default:
                    throw new System.Exception("Format '" + format + "' is not supported.");
            }

            return str;
        }

        public static string FormatFloat(JSNumber value, string format, NumberFormatInfo info)
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
            if (format == null || format == "g" || format == "G")
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
