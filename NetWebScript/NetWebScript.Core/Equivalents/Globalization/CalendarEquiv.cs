using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents.Globalization
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Globalization.Calendar))]
    public class CalendarEquiv 
    {
        // TODO: More complete API to allow non-gregorian calendars.
        // => Do not try to fit standard API, do the more effective based on JavaScript Date object

        public virtual int TwoDigitYearMax { get; set; }
    }
}
