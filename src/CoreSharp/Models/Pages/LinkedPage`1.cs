using System.Collections.Generic;

namespace CoreSharp.Models.Pages;

public class LinkedPage<TElement> : Page<TElement>
{
    // Constructors
    public LinkedPage(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable<TElement> items)
        : base(pageNumber, pageSize, totalItems, totalPages, items)
    {
    }

    public LinkedPage(Page<TElement> page, string previousPage, string nextPage)
        : this(page.PageNumber, page.PageSize, page.TotalItems, page.TotalPages, page.Items)
    {
        PreviousPage = previousPage;
        NextPage = nextPage;
    }

    // Properties
    public string PreviousPage { get; init; }
    public string NextPage { get; init; }
    public override bool HasPrevious
        => !string.IsNullOrWhiteSpace(PreviousPage);
    public override bool HasNext
        => !string.IsNullOrWhiteSpace(NextPage);
}
