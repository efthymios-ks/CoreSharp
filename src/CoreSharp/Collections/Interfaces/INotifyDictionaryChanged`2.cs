using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections.Interfaces;

public interface INotifyDictionaryChanged<TKey, TValue> : IDictionary<TKey, TValue>
{
    // Events
    event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;
}
