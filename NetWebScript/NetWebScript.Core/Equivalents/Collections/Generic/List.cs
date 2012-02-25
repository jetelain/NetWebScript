using System;
using System.Collections;
using System.Collections.Generic;

namespace NetWebScript.Equivalents.Collections.Generic
{
    [ScriptEquivalent(typeof(System.Collections.Generic.List<>))]
    [ScriptAvailable]
    internal class List<T> : IList<T>
    {
        private readonly Script.JSArray<T> data = new Script.JSArray<T>();

        public List()
        {

        }

        public List(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                data.Push(item);
            }
        }

        public void Add(T item)
        {
            data.Push(item);
        }

        public int Count
        {
            get { return data.Length; }
        }

        public void RemoveAt(int index)
        {
            data.Splice(index, 1);
        }

        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public class Enumerator : IEnumerator, IEnumerator<T>
        {
            private readonly List<T> list;
            private int index = -1;

            internal Enumerator(List<T> list)
            {
                this.list = list;
            }

            public T Current 
            { 
                get 
                {
                    return list.data[index];
                } 
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                index++;
                return index < list.data.Length;
            }

            object IEnumerator.Current
            {
                get { return list.data[index]; }
            }

            public void Reset()
            {
                index = -1;
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return NetWebScript.Script.JSArray<T>.IndexOf(item, data);
        }

        public void Insert(int index, T item)
        {
            data.Splice(index, 0, item);
        }

        #endregion

        #region ICollection<T> Members

        public void Clear()
        {
            data.Splice(0, data.Length);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index != -1)
            {
                data.Splice(index, 1);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion
    }
}
