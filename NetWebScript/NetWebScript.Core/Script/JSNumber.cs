using System;
using System.Globalization;
namespace NetWebScript.Script
{
    /// <summary>
    /// Wrapper object to work with numercial values.
    /// </summary>
    [Imported(Name = "Number", IgnoreNamespace = true)]
    public sealed class JSNumber
    {
        private readonly double value;

        private JSNumber(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// The largest positive representable number. The largest negative representable numebr is -MAX_VALUE.
        /// </summary>
        [PreserveCase]
        public static readonly JSNumber MAX_VALUE = new JSNumber(double.MaxValue);

        /// <summary>
        /// The smallest positive representable number -- that is, the positive numebr closest to zero (without actually being zero).
        /// </summary>
        [PreserveCase]
        public static readonly JSNumber MIN_VALUE = new JSNumber(double.Epsilon);

        /// <summary>
        /// "Not a Number" value.
        /// </summary>
        [PreserveCase]
        public static readonly JSNumber NaN = new JSNumber(double.NaN);

        /// <summary>
        /// Special value representing negative infinity (returned on overflow).
        /// </summary>
        [PreserveCase]
        public static readonly JSNumber NEGATIVE_INFINITY = new JSNumber(double.NegativeInfinity);

        /// <summary>
        /// Special value representing positive infinity (returned on overflow).
        /// </summary>
        [PreserveCase]
        public static readonly JSNumber POSITIVE_INFINITY = new JSNumber(double.PositiveInfinity);

        [ScriptAlias("isFinite")]
        public static bool IsFinite(JSNumber n)
        {
            return !double.IsInfinity(n.value) && !double.IsNaN(n.value);
        }

        [ScriptAlias("isNaN")]
        public static bool IsNaN(JSNumber n)
        {
            return double.IsNaN(n.value);
        }

        [ScriptAlias("parseFloat")]
        public static JSNumber ParseFloat(string s)
        {
            return new JSNumber(double.Parse(s, CultureInfo.InvariantCulture));
        }

        [ScriptAlias("parseInt")]
        public static JSNumber ParseInt(string s)
        {
            var trimmed = s.TrimStart();
            if (trimmed.StartsWith("0x", System.StringComparison.OrdinalIgnoreCase))
            {
                return ParseInt(trimmed.Substring(2), 16);
            }
            return ParseInt(trimmed, 10);
        }

        private static int Base36DigitToNumber(char digit)
        {
            if (digit >= '0' && digit <= '9')
            {
                return (int)digit - '0';
            }
            if (digit >= 'A' && digit <= 'Z')
            {
                return (int)digit - 55;
            }
            throw new ArgumentException();
        }

        private static int BaseToNumber(string value, int fromBase)
        {
            value = value.ToUpperInvariant();
            bool isNegative = false;
            if (value[0] == '-')
            {
                value = value.Substring(1);
                isNegative = true;
            }
            int intValue = 0;
            foreach (var c in value)
            {
                intValue *= fromBase;
                int digitValue =  Base36DigitToNumber(c);
                if (digitValue > fromBase)
                {
                    throw new ArgumentException();
                }
                intValue += digitValue;
            }
            if (isNegative)
            {
                return -intValue;
            }
            return intValue;
        }

        [ScriptAlias("parseInt")]
        public static JSNumber ParseInt(string s, int fromBase)
        {
            if (fromBase == 0)
            {
                return ParseInt(s);
            }
            try
            {
                if (fromBase != 2 /*&& fromBase != 8 */&& fromBase != 10 && fromBase != 16)
                {
                    return BaseToNumber(s, fromBase);
                }
                else
                {
                    return new JSNumber(Convert.ToInt64(s, fromBase));
                }
            }
            catch
            {
                return NaN;
            }
        }

        [ScriptAlias("Number")]
        public static JSNumber ToNumber(object o)
        {
            Date date = o as Date;
            if (date != null)
            {
                return new JSNumber(date - new Date(0));
            }
            return new JSNumber(Convert.ToDouble(o, CultureInfo.InvariantCulture));
        }

        public string ToExponential()
        {
            throw new NotImplementedException();
        }

        public string ToExponential(int fractionDigits)
        {
            throw new NotImplementedException();
        }

        public string ToFixed()
        {
            throw new NotImplementedException();
        }

        public string ToFixed(int fractionDigits)
        {
            throw new NotImplementedException();
        }

        public string ToPrecision()
        {
            throw new NotImplementedException();
        }

        public string ToPrecision(int precision)
        {
            throw new NotImplementedException();
        }

        public string ToString(int radix)
        {
            if (radix != 0)
            {
                return Convert.ToString((long)value, radix);
            }
            return ToString();
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public string ToLocalString()
        {
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [ScriptBody(Inline = "num")]
        public static implicit operator JSNumber(double num)
        {
            return new JSNumber(num);
        }

        [ScriptBody(Inline = "num")]
        public static implicit operator JSNumber(long num)
        {
            return new JSNumber((double)num);
        }

        [ScriptBody(Inline = "num")]
        public static implicit operator JSNumber(int num)
        {
            return new JSNumber((double)num);
        }

        [ScriptBody(Inline = "num")]
        public static implicit operator double(JSNumber num)
        {
            return num.value;
        }

        [ScriptBody(Inline = "num")]
        public static implicit operator int(JSNumber num)
        {
            return (int)num.value;
        }
        [ScriptBody(Inline = "num")]
        public static implicit operator long(JSNumber num)
        {
            return (long)num.value;
        }

        public static bool operator ==(JSNumber s1, object s2)
        {
            JSNumber num = s2 as JSNumber;
            if (s1 != null && num != null)
            {
                return s1.value == num.value;
            }
            return (double)s1 == (s2 as double?);
        }

        public static bool operator !=(JSNumber s1, object s2)
        {
            return (double)s1 != (s2 as double?);
        }

        public static bool operator ==(JSNumber s1, JSNumber s2)
        {
            return object.Equals(s1, s2);
        }

        public static bool operator !=(JSNumber s1, JSNumber s2)
        {
            return !object.Equals(s1, s2);
        }

        [ImportedExtension]
        public override int GetHashCode()
        {
            return (int)(((double)this) % 0x7fffffff);
        }

        [ImportedExtension]
        public override bool Equals(object obj)
        {
            return this == obj;
        }
    }
}

