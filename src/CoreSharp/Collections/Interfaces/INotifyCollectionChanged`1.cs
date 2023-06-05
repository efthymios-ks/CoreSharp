using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoreSharp.Collections.Interfaces;

public interface INotifyCollectionChanged<TValue> : ICollection<TValue>, INotifyCollectionChanged
{
    // Events
    new event EventHandler<NotifyCollectionChangedEventArgs<TValue>> CollectionChanged;
}
