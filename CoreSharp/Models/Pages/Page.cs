using System;
using System.Collections;
using System.Diagnostics;

namespace CoreSharp.Models.Pages
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Page
    {
        //Constructors
        public Page(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable items)
        {
            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), $"{nameof(pageNumber)} has to be positive.");
            else if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");
            else if (totalItems < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItems), $"{nameof(totalItems)} has to be positive and non-zero.");
            else if (totalPages < 0)
                throw new ArgumentOutOfRangeException(nameof(totalPages), $"{nameof(totalPages)} has to be positive and non-zero.");

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString();
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
        public IEnumerable Items { get; }
        public virtual bool HasPrevious
            => PageNumber > 0;
        public virtual bool HasNext
            => PageNumber < (TotalPages - 1);

        //Methods
        public override string ToString()
            => $"{nameof(PageNumber)}={PageNumber}, {nameof(PageSize)}={PageSize}";
    }
}
