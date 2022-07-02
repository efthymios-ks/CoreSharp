using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreSharp.Collections;

public class ObservableHashSet<TEntity> : ICollection<TEntity>, INotifyCollectionChanged
{
    //Fields
    private readonly HashSet<TEntity> _source;

    //Constructors
    public ObservableHashSet()
        : this(EqualityComparer<TEntity>.Default)
    {
    }

    public ObservableHashSet(IEnumerable<TEntity> source)
        : this(source, EqualityComparer<TEntity>.Default)
    {
    }

    public ObservableHashSet(IEqualityComparer<TEntity> equalityComparer)
        => _source = new(equalityComparer);

    public ObservableHashSet(IEnumerable<TEntity> source, IEqualityComparer<TEntity> equalityComparer)
        => _source = new(source, equalityComparer);

    public ObservableHashSet(int capacity)
        : this(capacity, EqualityComparer<TEntity>.Default)
    {
    }

    public ObservableHashSet(int capacity, IEqualityComparer<TEntity> equalityComparer)
        => _source = new(capacity, equalityComparer);

    //Properties
    public int Count
        => _source.Count;

    public bool IsReadOnly
        => false;

    //Events 
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    //Methods
    protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        _ = args ?? throw new ArgumentNullException(nameof(args));

        CollectionChanged?.Invoke(this, args);
    }

    public bool Contains(TEntity item)
        => _source.Contains(item);

    public void CopyTo(TEntity[] array, int arrayIndex)
        => _source.CopyTo(array, arrayIndex);

    public void Add(TEntity item)
    {
        if (!_source.Add(item))
            return;

        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, new[] { item }));
    }

    public void Clear()
    {
        if (_source.Count == 0)
            return;

        _source.Clear();
        OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
    }

    public bool Remove(TEntity item)
    {
        if (!_source.Remove(item))
            return false;

        OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, new[] { item }));
        return true;
    }

    public IEnumerator<TEntity> GetEnumerator()
        => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
