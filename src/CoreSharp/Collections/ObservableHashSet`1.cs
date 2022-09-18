using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreSharp.Collections;

public sealed class ObservableHashSet<TEntity> : HashSet<TEntity>, INotifyCollectionChanged
{
    // Events 
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    // Methods
    public new void Add(TEntity item)
    {
        if (!base.Add(item))
            return;

        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, new[] { item }));
    }

    public new bool Remove(TEntity item)
    {
        if (!base.Remove(item))
            return false;

        OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, new[] { item }));
        return true;
    }

    public new void Clear()
    {
        if (Count == 0)
            return;

        base.Clear();
        OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
    }

    private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        => CollectionChanged?.Invoke(this, args);
}