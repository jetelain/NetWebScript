using System.Globalization;
using NetWebScript.Script;
using System;

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
                case 'n':
                case 'N':
                    str = GroupIntegerNumber(((JSNumber)JSMath.Abs(value)).ToString(), info.NumberGroupSizes, info.NumberGroupSeparator);
                    if (precision == -1)
                    {
                        precision = info.NumberDecimalDigits;
                    }
                    if (precision > 0)
                    {
                        str = str + info.NumberDecimalSeparator + "".PadRight(precision, '0');
                    }
                    if (value < 0)
                    {
                        str = ApplyNumberNegativePattern(info.NumberNegativePattern, str, info.NegativeSign);
                    }
                    break;
                default:
                    throw new System.Exception("Format '" + format + "' is not supported.");
            }

            return str;
        }

        private static string ApplyNumberNegativePattern(int pattern, string number, string negativeSign)
        {
            switch(pattern)
            {
                case 0:
                    return "(" + number + ")";
                case 1:
                    return negativeSign + number;
                case 2:
                    return negativeSign + " " + number;
                case 3:
                    return number + negativeSign;
                case 4:
                    return number + " " + negativeSign;
            }
            throw new System.Exception("Invalid NumberNegativePattern");
        }

        public static string FormatFloat(JSNumber value, JSString format, NumberFormatInfo info)
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
                if (NeedNormalizeDecimal(info))
                {
                    return NormalizeDecimal(info, value.ToString());
                }
                return value.ToString();
            }

            char formatChar = (char)format.CharCodeAt(0);
            int precision = -1;
            if (format.Length > 1)
            {
                precision = JSNumber.ParseInt(format.Substr(1));
            }
            string str;
            switch (formatChar)
            {
                case 'e':
                case 'E':
                    if (precision == -1)
                    {
                        str = value.ToExponential();
                    }
                    else
                    {
                        str = value.ToExponential(precision);
                    }
                    if (formatChar == 'E')
                    {
                        str = str.ToUpperInvariant();
                    }
                    if (NeedNormalizeDecimal(info))
                    {
                        str = NormalizeDecimal(info, str);
                    }
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            return str;
        }

        private static bool NeedNormalizeDecimal(NumberFormatInfo info)
        {
            return info.NumberDecimalSeparator != "." || info.NegativeSign != "-" || info.PositiveSign != "+";
        }

        private static JSString NormalizeDecimal(NumberFormatInfo info, JSString str)
        {
            return str.Replace(new JSRegExp(@"\.\-\+", "g"), s =>
            {
                if (s == ".") return info.NumberDecimalSeparator;
                if (s == "-") return info.NegativeSign;
                return info.PositiveSign;
            });
        }

        private static string GroupDecimalNumber(JSString numberString, int[] groupSizes, string groupSep, string decimalSep) 
        {
            int idx = numberString.IndexOf(".");
            if (idx == -1)
            {
                return GroupIntegerNumber(numberString, groupSizes, groupSep);
            }
            return GroupIntegerNumber(numberString.Substr(0, idx), groupSizes, groupSep) + decimalSep + numberString.Substr(idx + 1);	
        }

        internal static string GroupIntegerNumber(JSString numberString, int[] groupSizes, string groupSep)
        {
            var curSize = groupSizes[0];
            var curGroupIndex = 1;
            var stringIndex = numberString.Length - 1;
            var ret = "";
            while (stringIndex >= 0)
            {
                if (curSize == 0 || curSize > stringIndex)
                {
                    return numberString.Substring(0, stringIndex + 1) + (ret.Length > 0 ? (groupSep + ret) : "");
                }
                ret = numberString.Substring(stringIndex - curSize + 1, stringIndex + 1) + (ret.Length > 0 ? (groupSep + ret) : "");
                stringIndex -= curSize;
                if (curGroupIndex < groupSizes.Length)
                {
                    curSize = groupSizes[curGroupIndex];
                    curGroupIndex++;
                }
            }
            return numberString.Substring(0, stringIndex + 1) + groupSep + ret;	
        }

        internal static JSNumber ParseInteger(JSString str, NumberFormatInfo info)
        {
            if (info.NegativeSign != "-")
            {
                str = str.Replace(info.NegativeSign, "-");
            }
            if (info.PositiveSign != "+")
            {
                str = str.Replace(info.PositiveSign, "+");
            }
            return JSNumber.ParseInt(str);
        }

        internal static JSNumber ParseInteger(JSString str, NumberStyles style, NumberFormatInfo info)
        {
            if (style == NumberStyles.Integer)
            {
                return ParseInteger(str, info);
            }
            throw new System.NotImplementedException();
        }


        internal static JSNumber ParseFloat(JSString str, NumberFormatInfo numberFormatInfo)
        {
            // FIXME: Implement the .Net spec
            return JSNumber.ParseFloat(str);
        }

        internal static JSNumber ParseFloat(JSString str, NumberStyles style, NumberFormatInfo info)
        {
            // FIXME: Implement the .Net spec
            if (style == NumberStyles.Integer)
            {
                return ParseInteger(str, info);
            }
            return JSNumber.ParseFloat(str);
        }
    }
}
