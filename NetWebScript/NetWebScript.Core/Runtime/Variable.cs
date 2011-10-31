using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Runtime
{
    [ScriptAvailable]
    public sealed class Variable : IRef
    {
        public object localValue;

        public object Set(object value)
        {
            return localValue = value;
        }

        public object Get()
        {
            return localValue;
        }
    }
}
