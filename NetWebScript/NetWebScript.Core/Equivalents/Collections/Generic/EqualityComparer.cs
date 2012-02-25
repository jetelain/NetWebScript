using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NetWebScript.Equivalents.Collections.Generic
{
    [ScriptEquivalent(typeof(System.Collections.Generic.EqualityComparer<>))]
    [ScriptAvailable]
    internal abstract class EqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        private static EqualityComparer<T> @default;

        public static EqualityComparer<T> Default
        {
            get
            {
                EqualityComparer<T> comparer = @default;
                if (comparer == null)
                {
                    comparer = new DefaultEqualityComparer<T>();
                    @default = comparer;
                }
                return comparer;
            }
        }
        
        public abstract bool Equals(T x, T y);

        public abstract int GetHashCode(T obj);

        bool IEqualityComparer.Equals(object x, object y)
        {
            throw new System.NotImplementedException();
            //return Equals((T)x, (T)y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            throw new System.NotImplementedException();
            //return GetHashCode((T)obj);
        }
    }
}
