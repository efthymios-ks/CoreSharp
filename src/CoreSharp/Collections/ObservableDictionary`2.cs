using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreSharp.Collections;

public sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
{
    // Properties
    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            var found = TryGetValue(key, out var previousValue);
            var equals = Equals(previousValue, value);

            // If found and equal, just return. 
            if (found && equals)
                return;

            // Based on documentation this adds or updates. 
            base[key] = value;

            // Found (and not equal), updated. 
            if (found)
                OnItemUpdated(key, previousValue, value);
            // Not found. 
            else
                OnItemAdded(key, value);
        }
    }

    // Events 
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    // Methods
    public new void Add(TKey key, TValue value)
        => TryAdd(key, value);

    public new bool TryAdd(TKey key, TValue value)
    {
        if (!base.TryAdd(key, value))
            return false;

        OnItemAdded(key, value);
        return true;
    }

    public new bool Remove(TKey key)
        => Remove(key, out var _);

    public new bool Remove(TKey key, out TValue removedValue)
    {
        if (!base.Remove(key, out removedValue))
            return false;

        OnItemRemoved(key, removedValue);
        return true;
    }

    public new void Clear()
    {
        if (Count == 0)
            return;

        base.Clear();
        OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
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

    private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        => CollectionChanged?.Invoke(this, args);
}
