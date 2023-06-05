namespace CoreSharp.Collections.Events;

public sealed class NotifyListChangedEventArgs<TValue> : NotifyCollectionChangedEventArgs<TValue>
{
    // Properties  
    public int Index { get; init; }
}