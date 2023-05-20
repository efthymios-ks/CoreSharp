using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSharp.Enumerables.SwitchCases;

public class AsyncSwitchCases<TKey, TResult> : IEnumerable<KeyValuePair<TKey, Task<TResult>>>
{
    // Fields 
    private readonly IDictionary<TKey, Task<TResult>> _cases;

    // Constructors
    public AsyncSwitchCases()
        : this(null)
    {
    }

    public AsyncSwitchCases(IEqualityComparer<TKey> equalityComparer)
        => _cases = new Dictionary<TKey, Task<TResult>>(equalityComparer);

    // Methods 
    public void Add(TKey key, Task<TResult> task)
        => _cases.Add(key, task);

    public void Add(TKey key, TResult result)
        => Add(key, Task.FromResult(result));

    public async Task<TResult> SwitchAsync(TKey key)
    {
        if (_cases.TryGetValue(key, out var task))
        {
            return await task;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }
    }

    public IEnumerator<KeyValuePair<TKey, Task<TResult>>> GetEnumerator()
        => _cases.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
