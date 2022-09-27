using CoreSharp.Collections.Events;
using System;
using System.Collections.Generic;

namespace CoreSharp.Collections.Interfaces;

public interface IObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    // Events
    event EventHandler<DictionaryChangedEventArgs<TKey, TValue>> Changed;
}
