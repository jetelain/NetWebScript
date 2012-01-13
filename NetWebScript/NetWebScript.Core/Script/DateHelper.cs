using System;
using NetWebScript.Equivalents;

namespace NetWebScript.Script
{
    /// <summary>
    /// Utility class to convert .Net DataTime from or to JavaScript Date
    /// </summary>
    [ScriptAvailable]
    public static class DateHelper
    {
        /// <summary>
        /// Convert a JavaScript Date to a local DateTime.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Date date)
        {
            return DateTimeEquiv.ToDateTime(date);
        }

        /// <summary>
        /// Convert a DateTime to JavaScript Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Date ToJSDate(this DateTime date)
        {
            return DateTimeEquiv.ToDate(date);
        }
    }
}
