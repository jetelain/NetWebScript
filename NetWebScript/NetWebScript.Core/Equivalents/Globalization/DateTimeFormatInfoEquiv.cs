using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.DateTimeFormatInfo))]
    public class DateTimeFormatInfoEquiv : IFormatProvider
    {
        public string[] AbbreviatedDayNames { get; set; }
        public string[] AbbreviatedMonthGenitiveNames { get; set; }
        public string[] AbbreviatedMonthNames { get; set; }
        public string AMDesignator { get; set; }
        //public System.Globalization.Calendar Calendar { get; set; }
        public System.Globalization.CalendarWeekRule CalendarWeekRule { get; set; }
        public string DateSeparator { get; set; }
        public string[] DayNames { get; set; }
        public DayOfWeek FirstDayOfWeek { get; set; }
        public string FullDateTimePattern { get; set; }
        //public static DateTimeFormatInfoEquiv InvariantInfo { get; }
        //public bool IsReadOnly { get; }
        public string LongDatePattern { get; set; }
        public string LongTimePattern { get; set; }
        public string MonthDayPattern { get; set; }
        public string[] MonthGenitiveNames { get; set; }
        public string[] MonthNames { get; set; }
        //public string NativeCalendarName { get; }
        public string PMDesignator { get; set; }
        public string RFC1123Pattern 
        { 
            get { return "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"; } 
        }
        public string ShortDatePattern { get; set; }
        public string[] ShortestDayNames { get; set; }
        public string ShortTimePattern { get; set; }
        public string SortableDateTimePattern
        {
            get
            {
                return "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
            }
        }
        public string TimeSeparator { get; set; }
        public string UniversalSortableDateTimePattern
        {
            get
            {
                return "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
            }
        }
        public string YearMonthPattern { get; set; }

        public object GetFormat(Type formatType)
        {
            if (object.Equals(formatType, typeof(DateTimeFormatInfo)))
            {
                return this;
            }
            return null;
        }

        public static DateTimeFormatInfo CurrentInfo 
        { 
            get 
            {
                return CultureInfo.CurrentCulture.DateTimeFormat;
            } 
        }

        public static DateTimeFormatInfo InvariantInfo
        {
            get
            {
                return CultureInfo.InvariantCulture.DateTimeFormat;
            }
        }

        public static DateTimeFormatInfo GetInstance(IFormatProvider provider)
        {
            DateTimeFormatInfo info = null;
            if (provider != null)
            {
                info = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
                if (info != null)
                {
                    return info;
                }
            }
            return CurrentInfo;
        }
    }
}
