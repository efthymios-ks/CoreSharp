using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CoreSharp.Collections;

public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged
{
    // Fields
    private readonly IDictionary<TKey, TValue> _source;

    // Constructors
    public ObservableDictionary()
        : this(Enumerable.Empty<KeyValuePair<TKey, TValue>>())
    {
    }

    public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        : this(dictionary.AsEnumerable())
    {
    }

    public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source)
        : this(source, EqualityComparer<TKey>.Default)
    {
    }

    public ObservableDictionary(IEqualityComparer<TKey> equalityComparer)
        : this(Enumerable.Empty<KeyValuePair<TKey, TValue>>(), equalityComparer)
    {
    }

    public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> equalityComparer)
        : this(dictionary.AsEnumerable(), equalityComparer)
    {
    }

    public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(source, equalityComparer);

    public ObservableDictionary(int capacity)
        : this(capacity, EqualityComparer<TKey>.Default)
    {
    }

    public ObservableDictionary(int capacity, IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(capacity, equalityComparer);

    // Properties
    public TValue this[TKey key]
    {
        get => _source[key];
        set
        {
            var previousValue = this[key];
            if (Equals(previousValue, value))
                return;

            _source[key] = value;
            OnItemUpdated(key, previousValue, value);
        }
    }

    public ICollection<TKey> Keys
        => _source.Keys;

    public ICollection<TValue> Values
        => _source.Values;

    public int Count
        => _source.Count;

    public bool IsReadOnly
        => _source.IsReadOnly;

    // Events 
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    // Methods 
    protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        _ = args ?? throw new ArgumentNullException(nameof(args));

        CollectionChanged?.Invoke(this, args);
    }

    private void OnItemAdded(TKey key, TValue value)
    {
        var pair = new KeyValuePair<TKey, TValue>(key, value);
        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, pair);
        OnCollectionChanged(args);
    }

    private void OnItemUpdated(TKey key, TValue previousValue, TValue newValue)
    {
        var previousPair = new KeyValuePair<TKey, TValue>(key, previousValue);
        var newPair = new KeyValuePair<TKey, TValue>(key, newValue);
        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newPair, previousPair);
        OnCollectionChanged(args);
    }

    private void OnItemRemoved(TKey key, TValue value)
    {
        var pair = new KeyValuePair<TKey, TValue>(key, value);
        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, pair);
        OnCollectionChanged(args);
    }

    private void OnReset()
        => OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));

    public bool TryGetValue(TKey key, out TValue value)
        => _source.TryGetValue(key, out value);

    public bool Contains(KeyValuePair<TKey, TValue> item)
        => _source.Contains(item);

    public bool ContainsKey(TKey key)
        => _source.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => _source.CopyTo(array, arrayIndex);

    public void Add(TKey key, TValue value)
        => Add(new(key, value));

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (!_source.TryAdd(item.Key, item.Value))
            return;

        OnItemAdded(item.Key, item.Value);
    }

    public void Clear()
    {
        if (_source.Count == 0)
            return;

        _source.Clear();
        OnReset();
    }

    public bool Remove(TKey key)
    {
        if (!_source.Remove(key, out var removedValue))
            return false;

        OnItemRemoved(key, removedValue);
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
        => Remove(item.Key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
