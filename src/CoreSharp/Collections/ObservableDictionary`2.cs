using CoreSharp.Collections.Events;
using CoreSharp.Collections.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.Collections;

public sealed class ObservableDictionary<TKey, TValue> : INotifyDictionaryChanged<TKey, TValue>
{
    // Fields 
    private readonly IDictionary<TKey, TValue> _source;

    // Constructors 
    public ObservableDictionary()
        => _source = new Dictionary<TKey, TValue>();

    public ObservableDictionary(int capacity)
        => _source = new Dictionary<TKey, TValue>(capacity);

    public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        => _source = new Dictionary<TKey, TValue>(collection);

    public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        => _source = new Dictionary<TKey, TValue>(dictionary);

    public ObservableDictionary(IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(equalityComparer);

    public ObservableDictionary(int capacity, IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(capacity, equalityComparer);

    public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(collection, equalityComparer);

    public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> equalityComparer)
        => _source = new Dictionary<TKey, TValue>(dictionary, equalityComparer);

    // Properties
    public ICollection<TKey> Keys
        => _source.Keys;

    public ICollection<TValue> Values
        => _source.Values;

    public int Count
        => _source.Count;

    public bool IsReadOnly
        => _source.IsReadOnly;

    public TValue this[TKey key]
    {
        get => _source[key];
        set
        {
            var found = TryGetValue(key, out var oldValue);
            var equals = Equals(oldValue, value);

            // If found and equal, just return. 
            if (found && equals)
            {
                return;
            }

            // Add or update. 
            _source[key] = value;

            // Found (and not equal), updated. 
            if (found)
            {
                OnDictionaryChanged(NotifyCollectionChangedAction.Replace, key, value, oldValue);
            }

            // Not found. 
            else
            {
                OnDictionaryChanged(NotifyCollectionChangedAction.Add, key, value);
            }
        }
    }

    // Events  
    public event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;

    // Methods 
    public void Add(TKey key, TValue value)
    {
        _source.Add(key, value);
        OnDictionaryChanged(NotifyCollectionChangedAction.Add, key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
        => Add(item.Key, item.Value);

    public bool Remove(TKey key)
    {
        if (!_source.Remove(key, out var value))
        {
            return false;
        }

        OnDictionaryChanged(NotifyCollectionChangedAction.Remove, key, oldValue: value);
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!_source.Remove(item))
        {
            return false;
        }

        OnDictionaryChanged(NotifyCollectionChangedAction.Remove, item.Key, oldValue: item.Value);
        return true;
    }

    public void Clear()
    {
        if (Count == 0)
        {
            return;
        }

        _source.Clear();
        OnDictionaryChanged(NotifyCollectionChangedAction.Reset);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _source.TryGetValue(key, out value);

    public bool ContainsKey(TKey key)
        => _source.ContainsKey(key);

    public bool Contains(KeyValuePair<TKey, TValue> item)
        => _source.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => _source.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private void OnDictionaryChanged(
        NotifyCollectionChangedAction action,
        TKey key = default,
        TValue newValue = default,
        TValue oldValue = default)
            => DictionaryChanged?.Invoke(this, new()
            {
                Action = action,
                Key = key,
                NewValue = newValue,
                OldValue = oldValue
            });
}
