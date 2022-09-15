using System.ComponentModel;
using System.Diagnostics;

namespace CoreSharp.Models.Pages;

public class PageOptions
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const int DefaultPageNumber = default;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const int DefaultPageSize = 20;

    // Properties 
    [DefaultValue(DefaultPageNumber)]
    public int PageNumber { get; set; } = DefaultPageNumber;

    [DefaultValue(DefaultPageSize)]
    public int PageSize { get; set; } = DefaultPageSize;
}
