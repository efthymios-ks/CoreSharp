using System;
using System.Collections.Generic;

namespace CoreSharp.EqualityComparers
{
    public class KeyEqualityComparer<TEntity, TKey> : IEqualityComparer<TEntity>
    {
        //Fields
        private readonly IEqualityComparer<TKey> _keyComparer;

        //Constructors
        public KeyEqualityComparer(Func<TEntity, TKey> keySelector)
            : this(keySelector, EqualityComparer<TKey>.Default)
        {
        }

        public KeyEqualityComparer(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
        }

        //Properties
        protected Func<TEntity, TKey> KeySelector { get; }

        //Methods 
        public bool Equals(TEntity left, TEntity right)
        {
            //Same reference 
            if (ReferenceEquals(left, right))
                return true;
            //Null 
            else if (left is null || right is null)
                return false;


            var leftKey = KeySelector(left);
            var rightKey = KeySelector(right);
            return _keyComparer.Equals(leftKey, rightKey);
        }

        public int GetHashCode(TEntity item)
        {
            if (item is null)
                return _keyComparer.GetHashCode(default);

            var key = KeySelector(item);
            return _keyComparer.GetHashCode(key);
        }
    }
}
