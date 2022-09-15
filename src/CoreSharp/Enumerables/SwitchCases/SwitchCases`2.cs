using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreSharp.Enumerables.SwitchCases;

public class SwitchCases<TKey, TResult> : IEnumerable<KeyValuePair<TKey, Func<TResult>>>
{
    // Fields 
    private readonly IDictionary<TKey, Func<TResult>> _cases;

    // Constructors
    public SwitchCases()
        : this(null)
    {
    }

    public SwitchCases(IEqualityComparer<TKey> equalityComparer)
        => _cases = new Dictionary<TKey, Func<TResult>>(equalityComparer);

    // Methods 
    public void Add(TKey key, Func<TResult> action)
        => _cases.Add(key, action);

    public void Add(TKey key, TResult result)
        => Add(key, () => result);

    public TResult Switch(TKey key)
    {
        if (_cases.TryGetValue(key, out var value))
            return value();
        else
            throw new ArgumentOutOfRangeException(nameof(key));
    }

    public IEnumerator<KeyValuePair<TKey, Func<TResult>>> GetEnumerator()
        => _cases.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
