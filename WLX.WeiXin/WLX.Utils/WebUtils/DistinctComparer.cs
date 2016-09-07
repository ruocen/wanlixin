using System;
using System.Collections.Generic;

namespace WLX.Utils.WebUtils
{
    public class DistinctComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;

        public DistinctComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}
