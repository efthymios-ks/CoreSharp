using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections.Interfaces;

public interface IObservableCollection<TItem> : ICollection<TItem>
{
    // Events
    event EventHandler<CollectionChangedEventArgs<TItem>> Changed;
}
