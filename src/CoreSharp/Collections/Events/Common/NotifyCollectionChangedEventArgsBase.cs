using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace CoreSharp.Collections.Events.Common;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class NotifyCollectionChangedEventArgsBase : EventArgs
{
    // Properties 
    private string DebuggerDisplay
        => $"Action={Action}";

    public NotifyCollectionChangedAction Action { get; init; }
}