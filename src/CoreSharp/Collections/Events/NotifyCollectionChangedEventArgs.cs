using CoreSharp.Collections.Events.Common;

namespace CoreSharp.Collections.Events;

public class NotifyCollectionChangedEventArgs<TValue> : NotifyCollectionChangedEventArgsBase
{
    // Properties 
    public TValue NewValue { get; init; }
    public TValue OldValue { get; init; }
}
