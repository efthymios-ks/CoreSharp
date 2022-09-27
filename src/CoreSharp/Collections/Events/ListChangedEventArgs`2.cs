namespace CoreSharp.Collections.Events;

public class ListChangedEventArgs<TKey, TItem> : CollectionChangedEventArgs<TItem>
{
    // Properties  
    public TKey Index { get; init; }
}
