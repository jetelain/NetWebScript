using System.Collections.Generic;

namespace NetWebScript.Script
{
    /// <summary>
    /// JavaScript / ECMAScript Array.
    /// </summary>
    /// <remarks>
    /// This class works both in plain CLR and in script for Unit Test purposes.
    /// For production code, it is recommanded to use standard .Net containers instead of this class (such as <see cref="System.Collections.Generic.List{T}"/>).
    /// </remarks>
    /// <typeparam name="T">Type of element</typeparam>
    [IgnoreNamespace, Imported(Name="Array")]
    public sealed class JSArray<T>
    {
        // This class is intend to have EXACTLY the same API as the native Array object as defined in ECMAScript
        // DO NOT ADD methods, properties of fields here.

        private readonly List<T> list = new List<T>();

        public void Push(T item)
        {
            list.Add(item);
        }

        public T Pop()
        {
            var idx = list.Count - 1;
            var value = list[idx];
            list.RemoveAt(idx);
            return value;
        }

        public T Shift()
        {
            var value = list[0];
            list.RemoveAt(0);
            return value;
        }

        public JSArray<T> Splice(int index, int count)
        {
            var removed = new JSArray<T>();
            while ( count > 0 )
            {
                removed.list.Add(list[index]);
                list.RemoveAt(index);
                count--;
            }
            return removed;
        }

        public JSArray<T> Splice(int index, int count, T value1)
        {
            var removed = new JSArray<T>();
            while (count > 0)
            {
                removed.list.Add(list[index]);
                list.RemoveAt(index);
                count--;
            }
            list.Insert(index, value1);
            return removed;
        }

        public JSArray<T> Splice(int index, int count, T value1, T value2)
        {
            var removed = new JSArray<T>();
            while (count > 0)
            {
                removed.list.Add(list[index]);
                list.RemoveAt(index);
                count--;
            }
            list.Insert(index, value1);
            list.Insert(index+1, value2);
            return removed;
        }

        public string Join(string separator)
        {
            return string.Join(separator, list);
        }

        public string Join()
        {
            return string.Join(string.Empty, list);
        }

        [IntrinsicProperty]
        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        [IntrinsicProperty]
        public int Length
        {
            get
            {
                return list.Count;
            }
        }

        [ScriptAlias("$.inArray")]
        public static int IndexOf(T element, JSArray<T> array)
        {
            return array.list.IndexOf(element);
        }
    }
}

