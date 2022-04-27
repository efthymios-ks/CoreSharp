using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreSharp.Enumerables.SwitchCases
{
    public class SwitchCases<TKey> : IEnumerable<KeyValuePair<TKey, Action>>
    {
        //Fields 
        private readonly IDictionary<TKey, Action> _cases;

        //Constructors
        public SwitchCases()
            : this(null)
        {
        }

        public SwitchCases(IEqualityComparer<TKey> equalityComparer)
            => _cases = new Dictionary<TKey, Action>(equalityComparer);

        //Methods 
        public void Add(TKey key, Action action)
            => _cases.Add(key, action);

        public void Switch(TKey key)
        {
            if (_cases.TryGetValue(key, out var value))
                value();
            else
                throw new ArgumentOutOfRangeException(nameof(key));
        }

        public IEnumerator<KeyValuePair<TKey, Action>> GetEnumerator()
            => _cases.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
