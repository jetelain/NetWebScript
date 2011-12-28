using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents
{
    [ScriptEquivalent(typeof(System.TimeSpan))]
    [ScriptAvailable]
    public sealed class TimeSpanEquiv
    {
        private readonly double totalMiliseconds;

        private TimeSpanEquiv(double totalMiliseconds)
        {
            this.totalMiliseconds = totalMiliseconds;
        }

        public double TotalMilliseconds
        {
            get { return totalMiliseconds; }
        }

        public static TimeSpanEquiv FromMilliseconds(double value)
        {
            return new TimeSpanEquiv(value);
        }
        public TimeSpanEquiv Add(TimeSpan ts)
        {
            return new TimeSpanEquiv(totalMiliseconds + ts.TotalMilliseconds);
        }
    }
}
