
using NetWebScript.Script;
using System.Globalization;
using System;
namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    internal class DateTimeParseFormat
    {
        internal readonly string regexp;
        internal readonly JSArray<string> groups;

        internal DateTimeParseFormat(string regexp, JSArray<string> groups)
        {
            this.regexp = regexp;
            this.groups = groups;
        }

        private static bool OutOfRange(int value, int low, int high)
        {
            return value < low || value > high;
        }

        internal Date Parse(string value, DateTimeFormatInfo dtf, bool isUTC)
        {

            value = value.Trim();
            var match = new JSRegExp(regexp).Exec(value);
            if (match == null)
            {
                return null;
            }

            int year = -1, month = 0, date = 1, weekDay = -1,
                hour = 0, hourOffset, min = 0, sec = 0, msec = 0, tzMinOffset = 0;
            bool pmHour = false, hasTimezone = false ;
            // iterate the format groups to extract and set the date fields.
            var jl = groups.Length;
            for (var j = 0; j < jl; j++)
            {
                var matchGroup = match[j + 1];
                if (matchGroup != null)
                {
                    var current = groups[j];
                    var clength = current.Length;
                    var matchInt = JSNumber.ParseInt(matchGroup, 10);
                    switch (current)
                    {
                        case "dd":
                        case "d":
                            // Day of month.
                            date = matchInt;
                            // check that date is generally in valid range, also checking overflow below.
                            if (OutOfRange(date, 1, 31)) return null;
                            break;
                        case "MMM":
                        case "MMMM":
                            month = GetMonthIndex(dtf, matchGroup, clength == 3);
                            if (OutOfRange(month, 0, 11)) return null;
                            break;
                        case "M":
                        case "MM":
                            // Month.
                            month = matchInt - 1;
                            if (OutOfRange(month, 0, 11)) return null;
                            break;
                        case "y":
                        case "yy":
                        case "yyyy":
                            year = clength < 4 ? ExpandYear(dtf, matchInt) : (int)matchInt;
                            if (OutOfRange(year, 0, 9999)) return null;
                            break;
                        case "h":
                        case "hh":
                            // Hours (12-hour clock).
                            hour = matchInt;
                            if (hour == 12) hour = 0;
                            if (OutOfRange(hour, 0, 11)) return null;
                            break;
                        case "H":
                        case "HH":
                            // Hours (24-hour clock).
                            hour = matchInt;
                            if (OutOfRange(hour, 0, 23)) return null;
                            break;
                        case "m":
                        case "mm":
                            // Minutes.
                            min = matchInt;
                            if (OutOfRange(min, 0, 59)) return null;
                            break;
                        case "s":
                        case "ss":
                            // Seconds.
                            sec = matchInt;
                            if (OutOfRange(sec, 0, 59)) return null;
                            break;
                        case "tt":
                        case "t":
                            // AM/PM designator.
                            // see if it is standard, upper, or lower case PM. If not, ensure it is at least one of
                            // the AM tokens. If not, fail the parse for this format.
                            pmHour = string.Equals(matchGroup, dtf.PMDesignator, StringComparison.OrdinalIgnoreCase);
                            if (
                                !pmHour && !string.Equals(matchGroup, dtf.AMDesignator, StringComparison.OrdinalIgnoreCase)
                            ) return null;
                            break;
                        case "f":
                        // Deciseconds.
                        case "ff":
                        // Centiseconds.
                        case "fff":
                            // Milliseconds.
                            msec = matchInt * (int)JSMath.Pow(10, 3 - clength);
                            if (OutOfRange(msec, 0, 999)) return null;
                            break;
                        case "ddd":
                        // Day of week.
                        case "dddd":
                            // Day of week.
                            weekDay = GetDayIndex(dtf, matchGroup, clength == 3);
                            if (OutOfRange(weekDay, 0, 6)) return null;
                            break;
                        case "zzz":
                            // Time zone offset in +/- hours:min.
                            var offsets = matchGroup.Split(':');
                            if (offsets.Length != 2) return null;
                            hourOffset = JSNumber.ParseInt(offsets[0], 10);
                            if (OutOfRange(hourOffset, -12, 13)) return null;
                            int minOffset = JSNumber.ParseInt(offsets[1], 10);
                            if (OutOfRange(minOffset, 0, 59)) return null;
                            tzMinOffset = (hourOffset * 60) + (matchGroup.StartsWith("-") ? -minOffset : minOffset);
                            hasTimezone = true;
                            break;
                        case "z":
                        case "zz":
                            // Time zone offset in +/- hours.
                            hourOffset = matchInt;
                            if (OutOfRange(hourOffset, -12, 13)) return null;
                            tzMinOffset = hourOffset * 60;
                            hasTimezone = true;
                            break;
                        //case "g": case "gg":
                        //    var eraName = matchGroup;
                        //    if ( !eraName || !cal.eras ) return null;
                        //    eraName = trim( eraName.toLowerCase() );
                        //    for ( var i = 0, l = cal.eras.length; i < l; i++ ) {
                        //        if ( eraName === cal.eras[i].name.toLowerCase() ) {
                        //            era = i;
                        //            break;
                        //        }
                        //    }
                        //    // could not find an era with that name
                        //    if ( era === null ) return null;
                        //    break;
                    }
                }
            }

            if (pmHour && hour < 12)
            {
                hour += 12;
            }

            Date result;

            if (hasTimezone)
            {
                min = min - tzMinOffset;

                hour = hour + (int)Math.Truncate(min / 60.0);
                min = min % 60;

                result = new Date(Date.UTC(year, month, date, hour, min, sec, msec));
            }
            else if (isUTC)
            {
                result = new Date(Date.UTC(year, month, date, hour, min, sec, msec));
            }
            else
            {
                result = new Date(year, month, date, hour, min, sec, msec);
            }

            // check to see if date overflowed for specified month (only checked 1-31 above).
            if (result.GetDate() != date)
            {
                return null;
            }
            // invalid day of week.
            if (weekDay != -1 && result.GetDay() != weekDay)
            {
                return null;
            }

            return result;
        }

        private static int UpperArrayIndex(string[] array, string value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].ToUpperInvariant() == value)
                {
                    return i;
                }
            }
            return -1;
        }

        internal static int GetDayIndex(DateTimeFormatInfo dtf, string dayName, bool abbr)
        {
            int ret;
            dayName = dayName.ToUpperInvariant();
            if ( abbr ) {
                ret = UpperArrayIndex(dtf.AbbreviatedDayNames, dayName);
			    if ( ret == -1 ) {
                    ret = UpperArrayIndex(dtf.ShortestDayNames, dayName);
			    }
		    }
		    else {
                ret = UpperArrayIndex(dtf.DayNames, dayName);
		    }
            return ret;
        }

        internal static int ExpandYear(DateTimeFormatInfo dtf, int year)
        {
            if ( year < 100 ) 
            {
                var currentYear = new Date().GetFullYear();
                year += currentYear - (currentYear % 100);
                if ( year > dtf.Calendar.TwoDigitYearMax ) 
                {
                    year -= 100;
                }
            }
		    return year;
        }

        internal static int GetMonthIndex(DateTimeFormatInfo dtf, string monthName, bool abbr)
        {
            int ret;
            monthName = monthName.ToUpperInvariant();
            if (abbr)
            {
                ret = UpperArrayIndex(dtf.AbbreviatedMonthNames, monthName);
                if (ret == -1)
                {
                    ret = UpperArrayIndex(dtf.AbbreviatedMonthGenitiveNames, monthName);
                }
            }
            else
            {
                ret = UpperArrayIndex(dtf.MonthNames, monthName);
                if (ret == -1)
                {
                    ret = UpperArrayIndex(dtf.MonthGenitiveNames, monthName);
                }
            }
            return ret;
        }
    }
}
