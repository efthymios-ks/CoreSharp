namespace CoreSharp.Collections.Events;

public sealed class NotifyDictionaryChangedEventArgs<TKey, TValue> : NotifyCollectionChangedEventArgs<TValue>
{
    // Properties 
    public TKey Key { get; init; }
}
