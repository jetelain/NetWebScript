using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Equivalents.Collections.Generic
{
    [ScriptEquivalent(typeof(System.Collections.Generic.Queue<>))]
    [ScriptAvailable]
    internal class Queue<T>
    {
        private readonly Script.JSArray<T> data = new Script.JSArray<T>();

        public void Enqueue(T item)
        {
            data.Push(item);
        }

        public T Dequeue()
        {
            return data.Shift();
        }

        public int Count
        {
            get { return data.Length; }
        }
    }
}
