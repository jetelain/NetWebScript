using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NetWebScript.Runtime
{
    [ScriptAvailable]
    public sealed class Variable : IRef
    {
        public object localValue;

        [DebuggerHidden]
        public object Set(object value)
        {
            return localValue = value;
        }

        [DebuggerHidden]
        public object Get()
        {
            return localValue;
        }
    }
}
