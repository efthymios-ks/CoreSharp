using CoreSharp.Collections.Events;
using CoreSharp.Collections.Interfaces;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections;

public sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IObservableDictionary<TKey, TValue>
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
                OnItemReplaced(key, previousValue, value);
            // Not found. 
            else
                OnItemAdded(key, value);
        }
    }
    // Events 
    public event EventHandler<DictionaryChangedEventArgs<TKey, TValue>> Changed;

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
        OnItemsCleared();
    }

    private void OnItemAdded(TKey key, TValue value)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Add,
            Index = key,
            NewItem = value
        });

    private void OnItemReplaced(TKey key, TValue oldValue, TValue newValue)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Replace,
            Index = key,
            OldItem = oldValue,
            NewItem = newValue
        });

    private void OnItemRemoved(TKey key, TValue value)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Remove,
            Index = key,
            OldItem = value
        });

    private void OnItemsCleared()
        => Changed?.Invoke(this, new() { Action = CollectionChangedAction.Clear });
}
