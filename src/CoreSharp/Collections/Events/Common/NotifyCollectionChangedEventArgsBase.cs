using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.Collections.Events.Common;

[SuppressMessage(
    "Minor Code Smell", "S3376:Attribute, EventArgs, and Exception type names should end with the type being extended",
    Justification = "<Pending>")]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class NotifyCollectionChangedEventArgsBase : EventArgs
{
    // Properties 
    private string DebuggerDisplay
        => $"Action={Action}";

    public NotifyCollectionChangedAction Action { get; init; }
}