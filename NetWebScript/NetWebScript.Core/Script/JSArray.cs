using System.Collections.Generic;
using System.Linq;

namespace NetWebScript.Script
{
    [ScriptAvailable]
    internal sealed class JSArrayEnumerator<T>: IEnumerator<T>
    {
        private readonly JSArray<T> array;
        private int index = -1;

        internal JSArrayEnumerator(JSArray<T> array)
        {
            this.array = array;
        }

        public T Current
        {
            get { return array[index]; }
        }

        public void Dispose()
        {

        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            index++;
            return index < array.Length;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    /// <summary>
    /// JavaScript / ECMAScript Array.
    /// </summary>
    /// <remarks>
    /// This class works both in plain CLR and in script for Unit Test purposes.
    /// For production code, it is recommanded to use standard .Net containers instead of this class (such as <see cref="System.Collections.Generic.List{T}"/>).
    /// </remarks>
    /// <typeparam name="T">Type of element</typeparam>
    [Imported(Name="Array", IgnoreNamespace=true)]
    public sealed class JSArray<T> : IEnumerable<T>
    {
        // This class is intend to have EXACTLY the same API as the native Array object as defined in ECMAScript
        // DO NOT ADD methods, properties of fields here.

        private readonly List<T> list = new List<T>();

        public JSArray()
        {

        }

        public JSArray(int size)
        {
            list = new List<T>(Enumerable.Repeat(default(T), size));
        }

        private JSArray(List<T> list)
        {
            this.list = list;
        }

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

        private int NormalizeIndex(int index)
        {
            return index < 0 ? Length + index : index;
        }

        public JSArray<T> Splice(int index, int count)
        {
            index = NormalizeIndex(index);
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
            index = NormalizeIndex(index);
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
            index = NormalizeIndex(index);
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

        public JSArray<T> Splice(int index, int count, T value1, T value2, T value3)
        {
            index = NormalizeIndex(index);
            var removed = new JSArray<T>();
            while (count > 0)
            {
                removed.list.Add(list[index]);
                list.RemoveAt(index);
                count--;
            }
            list.Insert(index, value1);
            list.Insert(index + 1, value2);
            list.Insert(index + 2, value3);
            return removed;
        }

        public JSArray<T> Splice(int index)
        {
            index = NormalizeIndex(index);
            return Splice(index, Length - index);
        }

        public string Join(string separator)
        {
            return string.Join(separator, list);
        }

        public string Join()
        {
            return string.Join(",", list);
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

        public JSArray<T> Reverse()
        {
            list.Reverse();
            return this;
        }

        [ScriptAlias("$.inArray")]
        public static int IndexOf(T element, JSArray<T> array)
        {
            return array.list.IndexOf(element);
        }

        [ScriptBody(Inline = "array")]
        public static implicit operator JSArray<T>(T[] array)
        {
            if (array != null)
            {
                var jsarray = new JSArray<T>();
                jsarray.list.AddRange(array);
                return jsarray;
            }
            return null;
        }

        [ScriptBody(Inline = "jsarray")]
        public static implicit operator T[](JSArray<T> jsarray)
        {
            if (jsarray != null)
            {
                return jsarray.list.ToArray();
            }
            return null;
        }

        [ImportedExtension]
        public IEnumerator<T> GetEnumerator()
        {
            return new JSArrayEnumerator<T>(this);
        }

        [ImportedExtension]
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public JSArray<T> Slice()
        {
            return new JSArray<T>(new List<T>(list));
        }

        public int Unshift(T value1)
        {
            list.Insert(0, value1);
            return list.Count;
        }

        [ScriptBody(Inline = "jsarray")]
        public static JSArray<T> New(params T[] jsarray)
        {
            return (JSArray<T>)jsarray;
        }
    }
}

