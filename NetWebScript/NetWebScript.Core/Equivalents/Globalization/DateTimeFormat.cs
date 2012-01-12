using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NetWebScript.Script;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    internal static class DateTimeFormat
    {
        private static JSRegExp CreateDateTokenRegExp()
        {
            return new JSRegExp(@"'.*?[^\\]'|dddd|ddd|dd|d|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f|zzz|zz|z", "g");
        }

        private static string GetExpandedFormat(JSString format, DateTimeFormatInfo dtf)
        {
            if (format == null)
            {
                return dtf.ShortDatePattern + " " + dtf.LongTimePattern;
            }
            else if (format.Length == 1)
            {
                return ExpandFormat((char)format.CharCodeAt(0), dtf);
            }
            else if (format.Length > 0 && format.CharCodeAt(0) == (int)'%')
            {
                return format.Substr(1);
            }
            return format;
        }

        internal static string FormatDate(JSString format, DateTimeFormatInfo dtf, Date date, DateTimeKind kind)
        {
            format = GetExpandedFormat(format, dtf);

            bool localDate = kind != DateTimeKind.Utc;

            var re = CreateDateTokenRegExp();
            var sb = new StringBuilder();
            re.LastIndex = 0;
            while (true) {
                var index = re.LastIndex;
                var match = re.Exec(format);
                sb.Append(format.Substring(index, match != null ? match.Index : format.Length));
                if (match == null) {
                    break;
                }
                if (localDate)
                {
                    sb.Append(PartLocalTime(match[0], dtf, date));
                }
                else
                {
                    sb.Append(PartUTCTime(match[0], dtf, date));
                }
            }
            return sb.ToString();
        }


        private static string ExpandFormat(char format, DateTimeFormatInfo dtf)
        {
            switch (format)
            {
                case 'f': return dtf.LongDatePattern + " " + dtf.ShortTimePattern;
                case 'F': return dtf.FullDateTimePattern;

                case 'd': return dtf.ShortDatePattern;
                case 'D': return dtf.LongDatePattern;

                case 't': return dtf.ShortTimePattern; 
                case 'T': return dtf.LongTimePattern;

                case 'g': return dtf.ShortDatePattern + " " + dtf.ShortTimePattern; 
                case 'G': return dtf.ShortDatePattern + " " + dtf.LongTimePattern;

                case 'M': return dtf.MonthDayPattern;
                case 'Y': return dtf.YearMonthPattern;

                case 's': return dtf.SortableDateTimePattern;
            }

            throw new System.Exception("Unknown format : "+format);
        }

        internal static string PartUTCTime(JSString fs, DateTimeFormatInfo dtf, Date dt)
        {
            switch ((string)fs)
            {
                case "dddd":
                    return dtf.DayNames[dt.GetUTCDay()];
                case "ddd":
                    return dtf.AbbreviatedDayNames[dt.GetUTCDay()];
                case "dd":
                    return dt.GetUTCDate().ToString().PadLeft(2, '0');
                case "d":
                    return dt.GetUTCDate().ToString();
                case "MMMM":
                    return dtf.MonthNames[dt.GetUTCMonth()];
                case "MMM":
                    return dtf.AbbreviatedMonthNames[dt.GetUTCMonth()];
                case "MM":
                    return (dt.GetUTCMonth() + 1).ToString().PadLeft(2, '0');
                case "M":
                    return (dt.GetUTCMonth() + 1).ToString();
                case "yyyy":
                    return dt.GetUTCFullYear().ToString();
                case "yy":
                    return (dt.GetUTCFullYear() % 100).ToString().PadLeft(2, '0');
                case "y":
                    return (dt.GetUTCFullYear() % 100).ToString();
                case "h":
                case "hh":
                    var h = dt.GetUTCHours() % 12;
                    if (h == 0)
                    {
                        return "12";
                    }
                    if (fs == "hh")
                    {
                        return h.ToString().PadLeft(2, '0');
                    }
                    return h.ToString();
                case "HH":
                    return dt.GetUTCHours().ToString().PadLeft(2, '0');
                case "H":
                    return dt.GetUTCHours().ToString();
                case "mm":
                    return dt.GetUTCMinutes().ToString().PadLeft(2, '0');
                case "m":
                    return dt.GetUTCMinutes().ToString();
                case "ss":
                    return dt.GetUTCSeconds().ToString().PadLeft(2, '0');
                case "s":
                    return dt.GetUTCSeconds().ToString();
                case "t":
                case "tt":
                    JSString tt = (dt.GetUTCHours() < 12) ? dtf.AMDesignator : dtf.PMDesignator;
                    if (fs == "t")
                    {
                        return tt.CharAt(0);
                    }
                    return tt;
                case "fff":
                    return dt.GetUTCMilliseconds().ToString().PadLeft(3, '0');
                case "ff":
                    return dt.GetUTCMilliseconds().ToString().PadLeft(3).Substring(0, 2);
                case "f":
                    return dt.GetUTCMilliseconds().ToString().PadLeft(3).Substring(0, 1);
                case "z":
                    var z = dt.GetTimezoneOffset() / 60;
                    return ((z >= 0) ? "-" : "+") + JSMath.Floor(JSMath.Abs(z)).ToString();
                case "zz":
                case "zzz":
                    var res = "+00";
                    if (fs == "zzz")
                    {
                        res += dtf.TimeSeparator + "00";
                    }
                    return res;
                default:
                    if (fs.CharAt(0) == "'")
                    {
                        return ((JSString)fs.Substr(1, fs.Length - 2)).Replace(new JSRegExp("\\'", "g"), "'");
                    }
                    return fs;
            }
        }

        private static string PartLocalTime(JSString fs, DateTimeFormatInfo dtf, Date dt)
        {
            switch ((string)fs)
            {
                case "dddd":
                    return dtf.DayNames[dt.GetDay()];
                case "ddd":
                    return dtf.AbbreviatedDayNames[dt.GetDay()];
                case "dd":
                    return dt.GetDate().ToString().PadLeft(2, '0');
                case "d":
                    return dt.GetDate().ToString();
                case "MMMM":
                    return dtf.MonthNames[dt.GetMonth()];
                case "MMM":
                    return dtf.AbbreviatedMonthNames[dt.GetMonth()];
                case "MM":
                    return (dt.GetMonth() + 1).ToString().PadLeft(2, '0');
                case "M":
                    return (dt.GetMonth() + 1).ToString();
                case "yyyy":
                    return dt.GetFullYear().ToString();
                case "yy":
                    return (dt.GetFullYear() % 100).ToString().PadLeft(2, '0');
                case "y":
                    return (dt.GetFullYear() % 100).ToString();
                case "h":
                case "hh":
                    var h = dt.GetHours() % 12;
                    if (h == 0)
                    {
                        return "12";
                    }
                    else if (fs == "hh")
                    {
                        return h.ToString().PadLeft(2, '0');
                    }
                    return h.ToString();
                case "HH":
                    return dt.GetHours().ToString().PadLeft(2, '0');
                case "H":
                    return dt.GetHours().ToString();
                case "mm":
                    return dt.GetMinutes().ToString().PadLeft(2, '0');
                case "m":
                    return dt.GetMinutes().ToString();
                case "ss":
                    return dt.GetSeconds().ToString().PadLeft(2, '0');
                case "s":
                    return dt.GetSeconds().ToString();
                case "t":
                case "tt":
                    JSString tt = (dt.GetHours() < 12) ? dtf.AMDesignator : dtf.PMDesignator;
                    if (fs == "t")
                    {
                        return tt.CharAt(0);
                    }
                    return tt;
                case "fff":
                    return dt.GetMilliseconds().ToString().PadLeft(3, '0');
                case "ff":
                    return dt.GetMilliseconds().ToString().PadLeft(3).Substring(0, 2);
                case "f":
                    return dt.GetMilliseconds().ToString().PadLeft(3).Substring(0, 1);
                case "z":
                    var z = dt.GetTimezoneOffset() / 60;
                    return ((z >= 0) ? "-" : "+") + JSMath.Floor(JSMath.Abs(z)).ToString();
                case "zz":
                case "zzz":
                    var zz = dt.GetTimezoneOffset() / 60;
                    var res = ((zz >= 0) ? '-' : '+') + JSMath.Floor(JSMath.Abs(zz)).ToString().PadLeft(2, '0');
                    if (fs == "zzz")
                    {
                        res += dtf.TimeSeparator + Math.Abs(dt.GetTimezoneOffset() % 60).ToString().PadLeft(2, '0');
                    }
                    return res;
                default:
                    if (fs.CharAt(0) == "'")
                    {
                        return ((JSString)fs.Substr(1, fs.Length - 2)).Replace(new JSRegExp("\\'", "g"), "'");
                    }
                    return fs;
            }
        }

        private static int AppendPreOrPostMatch ( string preMatch, StringBuilder strings ) 
        {
            var quoteCount = 0;
		    var escaped = false;
            var il = preMatch.Length;
	        for ( var i = 0; i < il; i++ ) {
		        var c = preMatch[i];
		        switch ( c ) {
			        case '\'':
				        if ( escaped ) {
					        strings.Append( "\'" );
				        }
				        else {
					        quoteCount++;
				        }
				        escaped = false;
				        break;
			        case '\\':
				        if ( escaped ) {
					        strings.Append( "\\" );
				        }
				        escaped = !escaped;
				        break;
			        default:
				        strings.Append( c );
				        escaped = false;
				        break;
		        }
	        }
	        return quoteCount;
        }

        internal static DateTimeParseFormat CreateDateParseRegExp(DateTimeFormatInfo dtf, string format) 
        {
            var expFormat = JSRegExpHelper.Escape(GetExpandedFormat(format, dtf));
		    var regexp = new StringBuilder();
		    var groups = new JSArray<string>();
		    var index = 0;
		    var quoteCount = 0;
		    var tokenRegExp = CreateDateTokenRegExp();
            NetWebScript.Script.JSRegExp.ExecResult match;

            regexp.Append("^");

		    while ( (match = tokenRegExp.Exec(expFormat)) != null ) 
            {
                var preMatch = expFormat.Substring(index, match.Index - index);
			    index = tokenRegExp.LastIndex;

			    quoteCount += AppendPreOrPostMatch( preMatch, regexp );
			    if ( (quoteCount % 2) == 1 ) {
				    regexp.Append( match[0] );
				    continue;
			    }

			    var m = match[ 0 ];
				string add;
			    switch ( m ) {
				    case "dddd": case "ddd":
				    case "MMMM": case "MMM":
                    //case "gg": case "g":
					    add = "(\\D+)";
					    break;
				    case "tt": case "t":
					    add = "(\\D*)";
					    break;
				    case "yyyy":
				    case "fff":
				    case "ff":
				    case "f":
                        add = "(\\d{" + m.Length + "})";
					    break;
				    case "dd": case "d":
				    case "MM": case "M":
				    case "yy": case "y":
				    case "HH": case "H":
				    case "hh": case "h":
				    case "mm": case "m":
				    case "ss": case "s":
					    add = "(\\d\\d?)";
					    break;
				    case "zzz":
					    add = "([+-]?\\d\\d?:\\d{2})";
					    break;
				    case "zz": case "z":
					    add = "([+-]?\\d\\d?)";
					    break;
				    case "/":
                        add = "(" + JSRegExpHelper.Escape(dtf.DateSeparator) + ")";
					    break;
				    default:
					    throw new System.Exception("Invalid date format pattern \'" + m + "\'.");
			    }
				regexp.Append( add );
			    groups.Push( m );
		    }
		    AppendPreOrPostMatch( expFormat.Substring(index), regexp );
		    regexp.Append( "$" );

            var regexpStr = regexp.ToString().Replace(new JSRegExp(@"\s+", "g"), @"\s+");

            return new DateTimeParseFormat(regexpStr, groups);
	    }

        internal static Date Parse(DateTimeFormatInfo dtf, string value, string format, bool isUTC)
        {
            return CreateDateParseRegExp(dtf, format).Parse(value, dtf, isUTC);
        }

        internal static Date ParseAny(DateTimeFormatInfo dtf, string value, string[] formats, bool isUTC)
        {
            Date date;
            foreach (var format in formats)
            {
                date = CreateDateParseRegExp(dtf, format).Parse(value, dtf, isUTC);
                if (date != null)
                {
                    return date;
                }
            }
            return null;
        }


    }
}
