using CoreSharp.Collections.Events;
using CoreSharp.Collections.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Collections;

[DebuggerDisplay("Count = {Count}")]
public sealed class ObservableList<TItem> : IObservableList<TItem>
{
    // Fields 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IList<TItem> _source;

    // Constructors
    public ObservableList()
        => _source = new List<TItem>();

    public ObservableList(IEnumerable<TItem> collection)
        => _source = new List<TItem>(collection);

    public ObservableList(int capacity)
        => _source = new List<TItem>(capacity);

    // Properties
    public TItem this[int index]
    {
        get => _source[index];
        set
        {
            var oldItem = GetItemOrDefault(index);
            if (Equals(oldItem, value))
            {
                return;
            }

            _source[index] = value;
            OnItemReplaced(index, oldItem, value);
        }
    }

    public int Count
        => _source.Count;

    public bool IsReadOnly
        => _source.IsReadOnly;

    // Events 
    public event EventHandler<ListChangedEventArgs<TItem>> Changed;

    // Methods 
    public void Add(TItem item)
    {
        _source.Add(item);

        OnItemAdded(Count - 1, item);
    }

    public void Insert(int index, TItem item)
    {
        _source.Insert(index, item);

        OnItemAdded(index, item);
    }

    public bool Remove(TItem item)
    {
        var index = IndexOf(item);
        if (index < 0)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        var itemToRemove = GetItemOrDefault(index);
        _source.RemoveAt(index);
        OnItemRemoved(index, itemToRemove);
    }

    public void Clear()
    {
        if (Count == 0)
        {
            return;
        }

        _source.Clear();
        OnItemsCleared();
    }

    public int IndexOf(TItem item)
        => _source.IndexOf(item);

    public bool Contains(TItem item)
        => _source.Contains(item);

    public void CopyTo(TItem[] array, int arrayIndex)
        => _source.CopyTo(array, arrayIndex);

    public IEnumerator<TItem> GetEnumerator()
        => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private TItem GetItemOrDefault(int index)
    {
        if (index < 0 || index >= Count)
        {
            return default;
        }

        return this[index];
    }

    private void OnItemAdded(int index, TItem item)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Add,
            Index = index,
            NewItem = item
        });

    private void OnItemReplaced(int index, TItem oldItem, TItem newItem)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Replace,
            Index = index,
            OldItem = oldItem,
            NewItem = newItem
        });

    private void OnItemRemoved(int index, TItem item)
        => Changed?.Invoke(this, new()
        {
            Action = CollectionChangedAction.Remove,
            Index = index,
            OldItem = item
        });

    private void OnItemsCleared()
        => Changed?.Invoke(this, new() { Action = CollectionChangedAction.Clear });
}
