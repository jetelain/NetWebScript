using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents.Threading
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.Threading.Thread))]
    public class Thread
    {
        private static Thread unique;
        public static Thread CurrentThread { get { if (unique == null) { unique = new Thread(); } return unique; } }
        public int ManagedThreadId { get { return 0; } }
    }
}
