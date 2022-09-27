using System;
using System.Diagnostics;

namespace CoreSharp.Collections.Events;

[DebuggerDisplay("Action = {Action}")]
public class CollectionChangedEventArgs<TItem> : EventArgs
{
    // Properties 
    public CollectionChangedAction Action { get; init; }
    public TItem NewItem { get; init; }
    public TItem OldItem { get; init; }
}
