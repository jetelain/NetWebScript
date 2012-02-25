using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents.Collections.Generic
{
    [ScriptEquivalent(typeof(System.Collections.Generic.KeyValuePair<,>))]
    [ScriptAvailable]
    internal sealed class KeyValuePair<TKey, TValue>
    {
        private readonly TKey key;
        private readonly TValue value;

        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key
        {
            get { return key; }
        }

        public TValue Value
        {
            get { return value; }
        }

    }
}
