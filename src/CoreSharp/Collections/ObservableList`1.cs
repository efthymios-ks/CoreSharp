using CoreSharp.Collections.Events;
using CoreSharp.Collections.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreSharp.Collections;

public sealed class ObservableList<TItem> : INotifyListChanged<TItem>
{
    // Fields
    private readonly IList<TItem> _source;

    // Constructors
    public ObservableList()
        => _source = new List<TItem>();

    public ObservableList(int capacity)
        => _source = new List<TItem>(capacity);

    public ObservableList(IEnumerable<TItem> collection)
        => _source = new List<TItem>(collection);

    // Properties 
    public int Count
        => _source.Count;

    public bool IsReadOnly
        => false;

    public TItem this[int index]
    {
        get => _source[index];
        set
        {
            var oldItem = _source[index];
            if (Equals(oldItem, value))
            {
                return;
            }

            _source[index] = value;
            OnListChanged(NotifyCollectionChangedAction.Replace, index, value, oldItem);
        }
    }

    // Events  
    public event EventHandler<NotifyListChangedEventArgs<TItem>> ListChanged;

    // Methods 
    public void Add(TItem item)
    {
        _source.Add(item);
        OnListChanged(NotifyCollectionChangedAction.Add, Count - 1, item);
    }

    public void Insert(int index, TItem item)
    {
        _source.Insert(index, item);
        OnListChanged(NotifyCollectionChangedAction.Add, index, item);
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
        var itemToRemove = _source[index];
        _source.RemoveAt(index);
        OnListChanged(NotifyCollectionChangedAction.Remove, index, oldValue: itemToRemove);
    }

    public void Clear()
    {
        if (Count == 0)
        {
            return;
        }

        _source.Clear();
        OnListChanged(NotifyCollectionChangedAction.Reset);
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

    private void OnListChanged(
        NotifyCollectionChangedAction action,
        int index = default,
        TItem newValue = default,
        TItem oldValue = default)
            => ListChanged?.Invoke(this, new()
            {
                Action = action,
                Index = index,
                NewValue = newValue,
                OldValue = oldValue
            });
}
