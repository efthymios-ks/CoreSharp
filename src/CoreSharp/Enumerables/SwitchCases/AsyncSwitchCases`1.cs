using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSharp.Enumerables.SwitchCases
{
    public class AsyncSwitchCases<TKey> : IEnumerable<KeyValuePair<TKey, Task>>
    {
        //Fields 
        private readonly IDictionary<TKey, Task> _cases;

        //Constructors
        public AsyncSwitchCases()
            : this(null)
        {
        }

        public AsyncSwitchCases(IEqualityComparer<TKey> equalityComparer)
            => _cases = new Dictionary<TKey, Task>(equalityComparer);

        //Methods 
        public void Add(TKey key, Task task)
            => _cases.Add(key, task);

        public async Task SwitchAsync(TKey key)
        {
            if (_cases.TryGetValue(key, out var task))
                await task;
            else
                throw new ArgumentOutOfRangeException(nameof(key));
        }

        public IEnumerator<KeyValuePair<TKey, Task>> GetEnumerator()
            => _cases.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
