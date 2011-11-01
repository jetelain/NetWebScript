using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents.Collections.Generic
{
    [ScriptEquivalent(typeof(System.Collections.Generic.Dictionary<,>))]
    [ScriptAvailable]
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private class Entry
        {
            public TKey key;
            public TValue value;
        }

        private readonly JSArray<Entry>[] data = new JSArray<Entry>[32];
        private readonly JSArray<Entry> all = new JSArray<Entry>();
        private readonly IEqualityComparer<TKey> comparer;

        public Dictionary()
            : this(0, null)
        {
        }
        
        public Dictionary(int capacity)
            : this(capacity, null)
        {
        }
        
        public Dictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }
        
        public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.comparer = comparer == null ? EqualityComparer<TKey>.Default : comparer;
        }

        public Dictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }
        
        public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this((dictionary != null) ? dictionary.Count : 0, comparer)
        {
            foreach (var current in dictionary)
            {
                Add(current.Key, current.Value);
            }
        }

        private void Set(TKey key, TValue value, bool failIfExists)
        {
            var hashCode = comparer.GetHashCode(key);
            var segmentPos =  hashCode % data.Length;
            var segment = data[segmentPos];
            Entry entry = null;
            if (segment == null)
            {
                segment = data[segmentPos] = new JSArray<Entry>();
            }
            else if (segment.Length != 0)
            {
                for (int i = 0; i < segment.Length; ++i)
                {
                    if (comparer.Equals(segment[i].key, key))
                    {
                        if (failIfExists)
                        {
                            throw new System.Exception("Duplicate key");
                        }
                        entry = segment[i];
                        break;
                    }
                }
            }
            if (entry == null)
            {
                entry = new Entry();
                entry.key = key;
                segment.Push(entry);
                all.Push(entry);
            }
            entry.value = value;
        }

        private JSArray<Entry> GetSegment(TKey key)
        {
            var hashCode = comparer.GetHashCode(key);
            return data[hashCode % data.Length];
        }

        private Entry GetEntry(JSArray<Entry> segment, TKey key)
        {
            if (segment != null)
            {
                for (int i = 0; i < segment.Length; ++i)
                {
                    if (comparer.Equals(segment[i].key, key))
                    {
                        return segment[i];
                    }
                }
            }
            return null;
        }

        private Entry GetEntry(TKey key)
        {
            return GetEntry(GetSegment(key), key);
        }

        public void Add(TKey key, TValue value)
        {
            Set(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            return GetEntry(key) != null;
        }

        public ICollection<TKey> Keys
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Remove(TKey key)
        {
            var segment = GetSegment(key);
            if (segment != null)
            {
                for (int i = 0; i < segment.Length; ++i)
                {
                    if (comparer.Equals(segment[i].key, key))
                    {
                        var entry = segment[i];
                        segment.Splice(i, 1);

                        for (int j = 0; j < all.Length; ++j)
                        {
                            if (all[j] == entry)
                            {
                                all.Splice(j, 1);
                                break;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var entry = GetEntry(key);
            if (entry != null)
            {
                value = entry.value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public ICollection<TValue> Values
        {
            get { throw new System.NotImplementedException(); }
        }

        public TValue this[TKey key]
        {
            get
            {
                var entry = GetEntry(key);
                if (entry != null)
                {
                    return entry.value;
                }
                throw new System.Exception("Missing entry");
            }
            set
            {
                Set(key, value, false);
            }
        }

        public void Add(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            all.Splice(0, all.Length);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = null;
            }
        }

        public bool Contains(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return TryGetValue(item.Key, out value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public void CopyTo(System.Collections.Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public int Count
        {
            get { return all.Length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(System.Collections.Generic.KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (TryGetValue(item.Key, out value) && EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                Remove(item.Key);
                return true;
            }
            return false;
        }

        public IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
