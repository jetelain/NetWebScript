using System;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    /// <summary>
    /// Implementation of string.Format and assimilated functions.
    /// </summary>
    [ScriptAvailable]
    internal static class StringFormat
    {
        internal static string Format(IFormatProvider provider, JSString format, object[] parameters)
        {
            ICustomFormatter customFormatter = null;
            if (provider != null)
            {
                customFormatter = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
            }
            return Format(customFormatter, provider, format, parameters);
        }
        
        private static string Format(ICustomFormatter customFormatter, IFormatProvider provider, JSString format, object[] parameters)
        {
            string result = "";
            int pos;
            int lastpos = 0;
            while (lastpos < format.Length)
            {
                pos = NextPos(format, lastpos);
                if (pos != -1)
                {
                    if (pos + 1 == format.Length)
                    {
                        throw new System.Exception("Premature end of string");
                    }
                    int code = format.CharCodeAt(pos);
                    if (code == format.CharCodeAt(pos + 1))
                    {
                        result += format.Substring(lastpos, pos+1);
                        lastpos = pos + 2;
                        continue;
                    }
                    else if (code == '}')
                    {
                        throw new System.Exception("Invalid position of a '}'");
                    }
                    result += format.Substring(lastpos, pos);
                    pos++;

                    // Here we make a heavy simplification : we suppose that argument format does not contains escapped { or }
                    int end = format.IndexOf("}", pos);
                    if (end == -1)
                    {
                        throw new System.Exception("No '}' found");
                    }
                    result += Argument(customFormatter, provider, format, pos, end, parameters);
                    lastpos = end + 1;
                }
                else
                {
                    result += format.Substring(lastpos, format.Length);
                    lastpos = format.Length;
                }
            }
            return result;
        }

        private static string Argument(ICustomFormatter customFormatter, IFormatProvider provider, JSString format, int defStart, int defEnd, object[] parameters)
        {
            int numEnd = defEnd, alignEnd = -1;
            
            int colon = format.IndexOf(":", defStart);
            if (colon > defEnd)
            {
                colon = -1;
            }
            else if (colon != -1)
            {
                numEnd = colon;
            }

            int comma = format.IndexOf(",", defStart);
            if (comma != -1 && comma < numEnd)
            {
                alignEnd = numEnd;
                numEnd = comma;
            }

            int argnNum = JSNumber.ParseInt(format.Substring(defStart, numEnd));

            int argAlignment = 0;
            if (alignEnd != -1)
            {
                argAlignment = JSNumber.ParseInt(format.Substring(numEnd + 1, alignEnd));
            }

            string argFormat = null;
            if (colon != -1)
            {
                argFormat = format.Substring(colon + 1, defEnd);
            }

            var result = FormatArgument(customFormatter, provider, parameters[argnNum], argFormat);
            if (argAlignment != 0)
            {
                return Align(result, argAlignment);
            }
            return result;
        }

        private static string Align(string result, int argAlignment)
        {
            int length = (int)JSMath.Abs(argAlignment);
            if (length > result.Length)
            {
                if (argAlignment < 0)
                {
                    return result.PadRight(length);
                }
                else
                {
                    return result.PadLeft(length);
                }
            }
            return result;
        }

        private static string FormatArgument(ICustomFormatter customFormatter, IFormatProvider provider, object arg, string argFormat)
        {
            string result = null;
            if (customFormatter != null)
            {
                result = customFormatter.Format(argFormat, arg, provider);
            }
            if (result == null)
            {
                IFormattable formattable = arg as IFormattable;
                if (formattable != null)
                {
                    result = formattable.ToString(argFormat, provider);
                }
                else
                {
                    if (arg != null)
                    {
                        result = arg.ToString();
                    }
                }
            }
            if (result == null)
            {
                return "";
            }
            return result;
        }


        private static int NextPos(JSString format, int startPos)
        {
            int a = format.IndexOf("{", startPos);
            int b = format.IndexOf("}", startPos);
            if (b != -1 && (b < a || a == -1))
            {
                return b;
            }
            return a;
        }
    }
}
