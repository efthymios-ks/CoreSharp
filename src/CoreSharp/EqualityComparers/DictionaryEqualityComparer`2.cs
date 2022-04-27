using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.EqualityComparers
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEqualityComparer<TKey> _keyComparer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEqualityComparer<TValue> _valueComparer;

        //Constructors 
        public DictionaryEqualityComparer()
            : this(EqualityComparer<TKey>.Default, EqualityComparer<TValue>.Default)
        {
        }

        public DictionaryEqualityComparer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
            _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
        }

        //Methods
        public bool Equals(IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
        {
            //Same reference 
            if (left == right)
                return true;
            //Null 
            else if (left is null || right is null)
                return false;

            //Keys don't match
            var keysComparer = new EnumerableEqualityComparer<TKey>(_keyComparer);
            if (!keysComparer.Equals(left.Keys, right.Keys))
                return false;

            //Values don't match 
            foreach (var (key, value) in left)
            {
                var rightValue = right[key];
                if (!_valueComparer.Equals(value, rightValue))
                    return false;
            }

            return true;
        }

        public int GetHashCode(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary is null)
                return _valueComparer.GetHashCode(default);

            var hash = new HashCode();
            foreach (var (_, value) in dictionary)
                hash.Add(value, _valueComparer);
            return hash.ToHashCode();
        }
    }
}
