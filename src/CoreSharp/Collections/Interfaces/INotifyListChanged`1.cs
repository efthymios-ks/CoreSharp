using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections.Interfaces;

public interface INotifyListChanged<TValue> : IList<TValue>
{
    // Events
    event EventHandler<NotifyListChangedEventArgs<TValue>> ListChanged;
}
