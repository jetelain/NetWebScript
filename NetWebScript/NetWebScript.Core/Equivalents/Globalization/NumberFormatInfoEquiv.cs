using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.NumberFormatInfo))]
    public class NumberFormatInfoEquiv : IFormatProvider
    {
        public int CurrencyDecimalDigits { get; set; }

        public string CurrencyDecimalSeparator { get; set; }

        public string CurrencyGroupSeparator { get; set; }

        public int[] CurrencyGroupSizes { get; set; }

        public int CurrencyNegativePattern { get; set; }

        public int CurrencyPositivePattern { get; set; }

        public string CurrencySymbol { get; set; }

        public System.Globalization.DigitShapes DigitSubstitution { get; set; }

        public static NumberFormatInfoEquiv InvariantInfo { get { return CultureInfoEquiv.InvariantCulture.NumberFormat; } }

        //public bool IsReadOnly { get; }

        public string NaNSymbol { get; set; }

        public string[] NativeDigits { get; set; }

        public string NegativeInfinitySymbol { get; set; }

        public string NegativeSign { get; set; }

        public int NumberDecimalDigits { get; set; }

        public string NumberDecimalSeparator { get; set; }

        public string NumberGroupSeparator { get; set; }

        public int[] NumberGroupSizes { get; set; }

        public int NumberNegativePattern { get; set; }

        public int PercentDecimalDigits { get; set; }

        public string PercentDecimalSeparator { get; set; }

        public string PercentGroupSeparator { get; set; }

        public int[] PercentGroupSizes { get; set; }

        public int PercentNegativePattern { get; set; }

        public int PercentPositivePattern { get; set; }

        public string PercentSymbol { get; set; }

        public string PerMilleSymbol { get; set; }

        public string PositiveInfinitySymbol { get; set; }

        public string PositiveSign { get; set; }

        public object GetFormat(Type formatType)
        {
            if (object.Equals(formatType,typeof(NumberFormatInfo)))
            {
                return this;
            }
            return null;
        }

        public static NumberFormatInfo CurrentInfo
        {
            get
            {
                return CultureInfo.CurrentCulture.NumberFormat;
            }
        }

        public static NumberFormatInfo GetInstance(IFormatProvider provider)
        {
            NumberFormatInfo info = null;
            if (provider != null)
            {
                info = (NumberFormatInfo)provider.GetFormat(typeof(NumberFormatInfo));
                if (info != null)
                {
                    return info;
                }
            }
            return CurrentInfo;
        }

        public NumberFormatInfoEquiv()
        {

        }

        [ScriptBody(Inline = "info")]
        public NumberFormatInfoEquiv(NumberFormatInfo info)
        {
            CurrencyDecimalDigits = info.CurrencyDecimalDigits;
            CurrencyDecimalSeparator = info.CurrencyDecimalSeparator;
            CurrencyGroupSeparator = info.CurrencyGroupSeparator;
            CurrencyGroupSizes = info.CurrencyGroupSizes;
            CurrencyNegativePattern = info.CurrencyNegativePattern;
            CurrencyPositivePattern = info.CurrencyPositivePattern;
            CurrencySymbol = info.CurrencySymbol;
            DigitSubstitution = info.DigitSubstitution;
            NaNSymbol = info.NaNSymbol;
            NegativeInfinitySymbol = info.NegativeInfinitySymbol;
            NegativeSign = info.NegativeSign;
            NumberDecimalDigits = info.NumberDecimalDigits;
            NumberDecimalSeparator = info.NumberDecimalSeparator;
            NumberGroupSeparator = info.NumberGroupSeparator;
            NumberGroupSizes = info.NumberGroupSizes;
            NumberNegativePattern = info.NumberNegativePattern;
            PercentDecimalDigits = info.PercentDecimalDigits;
            PercentDecimalSeparator = info.PercentDecimalSeparator;
            PercentGroupSeparator = info.PercentGroupSeparator;
            PercentGroupSizes = info.PercentGroupSizes;
            PercentNegativePattern = info.PercentNegativePattern;
            PercentPositivePattern = info.PercentPositivePattern;
            PercentSymbol = info.PercentSymbol;
            PerMilleSymbol = info.PerMilleSymbol;
            PositiveInfinitySymbol = info.PositiveInfinitySymbol;
            PositiveSign = info.PositiveSign;
        }


    }
}
