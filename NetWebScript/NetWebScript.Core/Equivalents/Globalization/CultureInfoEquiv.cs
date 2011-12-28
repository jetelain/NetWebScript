using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.CultureInfo))]
    public class CultureInfoEquiv : IFormatProvider
    {
        public virtual DateTimeFormatInfo DateTimeFormat { get; set; }

        public virtual NumberFormatInfo NumberFormat { get; set; }

        public object GetFormat(Type formatType)
        {
            if (object.Equals(formatType, typeof(System.Globalization.DateTimeFormatInfo)))
            {
                return DateTimeFormat;
            }
            if (object.Equals(formatType, typeof(System.Globalization.NumberFormatInfo)))
            {
                return NumberFormat;
            }
            return null;
        }
    }
}
