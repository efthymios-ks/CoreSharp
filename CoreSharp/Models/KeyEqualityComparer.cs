using System;
using System.Collections.Generic;

namespace CoreSharp.Models
{
    public class KeyEqualityComparer<TItem, TKey> : IEqualityComparer<TItem>
    {
        //Constructors
        public KeyEqualityComparer(Func<TItem, TKey> keySelector)
            => KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        //Properties
        protected Func<TItem, TKey> KeySelector { get; }

        //Methods
        public virtual bool Equals(TItem left, TItem right)
            => EqualityComparer<TKey>.Default.Equals(KeySelector(left), KeySelector(right));

        public int GetHashCode(TItem item)
            => KeySelector(item).GetHashCode();
    }
}
