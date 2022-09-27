using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections.Interfaces;

public interface IObservableList<TItem> : IList<TItem>
{
    // Events
    event EventHandler<ListChangedEventArgs<TItem>> Changed;
}
