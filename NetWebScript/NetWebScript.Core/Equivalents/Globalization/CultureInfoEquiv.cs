﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.CultureInfo))]
    internal class CultureInfoEquiv : IFormatProvider
    {
        public virtual string Name { get; internal set; }

        public virtual bool IsNeutralCulture { get; internal set; }

        public virtual DateTimeFormatInfoEquiv DateTimeFormat { get; set; }

        public virtual NumberFormatInfoEquiv NumberFormat { get; set; }

        private static CultureInfoEquiv currentCulture;
        private static readonly CultureInfoEquiv invariantCulture;

        public static CultureInfoEquiv CurrentCulture 
        { 
            get 
            {
                if (currentCulture == null)
                {
                    currentCulture = GetCulture("en-US");
                }
                return currentCulture;
            } 
            set
            {
                currentCulture = value;
            }
        }

        public static CultureInfoEquiv InvariantCulture
        {
            get
            {
                return invariantCulture;
            }
        }

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

        

        private static readonly JSArray<CultureInfoEquiv> cultures;

        internal static CultureInfoEquiv GetCulture(string cultureName)
        {
            var culture = cultures.FirstOrDefault(c => c.Name == cultureName);
            if (culture == null )
            {
                culture = invariantCulture;
            }
            return culture;
        }

        internal static void AddCulture(CultureInfoEquiv culture)
        {
            cultures.Push(culture);
        }

        static CultureInfoEquiv()
        {
            invariantCulture = new CultureInfoEquiv()
            {
                Name = "",
                IsNeutralCulture = true,
                NumberFormat = new NumberFormatInfoEquiv()
                {
                    CurrencyDecimalDigits = 2,
                    CurrencyDecimalSeparator = ".",
                    CurrencyGroupSeparator = ",",
                    CurrencyGroupSizes = new[] { 3 },
                    CurrencyNegativePattern = 0,
                    CurrencyPositivePattern = 0,
                    CurrencySymbol = "¤",
                    DigitSubstitution = DigitShapes.None,
                    NaNSymbol = "NaN",
                    NegativeInfinitySymbol = "-Infinity",
                    NegativeSign = "-",
                    NumberDecimalDigits = 2,
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ",",
                    NumberGroupSizes = new[] { 3 },
                    NumberNegativePattern = 1,
                    PercentDecimalDigits = 2,
                    PercentDecimalSeparator = ".",
                    PercentGroupSeparator = ",",
                    PercentGroupSizes = new[] { 3 },
                    PercentNegativePattern = 0,
                    PercentPositivePattern = 0,
                    PercentSymbol = "%",
                    PerMilleSymbol = "‰",
                    PositiveInfinitySymbol = "Infinity",
                    PositiveSign = "+"
                },
                DateTimeFormat = new DateTimeFormatInfoEquiv()
                {
                    AbbreviatedDayNames = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" },
                    AbbreviatedMonthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                    AMDesignator = "AM",
                    CalendarWeekRule = CalendarWeekRule.FirstDay,
                    DateSeparator = "/",
                    DayNames = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
                    FirstDayOfWeek = DayOfWeek.Sunday,
                    FullDateTimePattern = "dddd, dd MMMM yyyy HH:mm:ss",
                    LongDatePattern = "dddd, dd MMMM yyyy",
                    LongTimePattern = "HH:mm:ss",
                    MonthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                    PMDesignator = "PM",
                    ShortDatePattern = "MM/dd/yyyy",
                    ShortestDayNames = new[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" },
                    ShortTimePattern = "HH:mm",
                    TimeSeparator = ":",
                    YearMonthPattern = "yyyy MMMM",
                    Calendar = new CalendarEquiv() {  TwoDigitYearMax = 2029 },
                    MonthDayPattern = "MMMM dd"
                }
            };

            cultures = new JSArray<CultureInfoEquiv>();
            cultures.Push(invariantCulture);
        }
        
        private CultureInfoEquiv()
        {

        }

        public CultureInfoEquiv(string name)
        {
            var data = GetCulture(name);
            if (data == null)
            {
                throw new System.Exception("CultureNotFoundException");
            }
            Name = data.Name;
            IsNeutralCulture = data.IsNeutralCulture;
            DateTimeFormat = data.DateTimeFormat;
            NumberFormat = data.NumberFormat;
        }

        [ScriptBody(Inline = "info")]
        public CultureInfoEquiv(CultureInfo info)
        {
            Name = info.Name;
            IsNeutralCulture = info.IsNeutralCulture;
            DateTimeFormat = new DateTimeFormatInfoEquiv(info.DateTimeFormat);
            NumberFormat = new NumberFormatInfoEquiv(info.NumberFormat);
        }

        public static CultureInfoEquiv[] GetCultures(CultureTypes types)
        {
            bool includeNeutral = (types & CultureTypes.NeutralCultures) == CultureTypes.NeutralCultures;
            bool includeSpecific = (types & CultureTypes.SpecificCultures) == CultureTypes.SpecificCultures;
            if (includeNeutral && includeSpecific)
            {
                return cultures;
            }
            JSArray<CultureInfoEquiv> list = new JSArray<CultureInfoEquiv>();
            if (cultures != null)
            {
                foreach (var culture in cultures)
                {
                    if (culture.IsNeutralCulture)
                    {
                        if (includeNeutral)
                        {
                            list.Push(culture);
                        }
                    }
                    else if (includeSpecific)
                    {
                        list.Push(culture);
                    }
                }
            }
            return list;
        }
    }
}
