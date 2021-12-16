using System;
using System.Collections;
using System.Diagnostics;

namespace CoreSharp.Models.Pages
{
    [global::Newtonsoft.Json.JsonObject]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Page : IEnumerable
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
        [global::Newtonsoft.Json.JsonProperty]
        internal IEnumerable Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }

        //Methods
        public override string ToString()
            => $"Page Number= {PageNumber}, Page Size={PageSize}";

        public IEnumerator GetEnumerator()
            => Items.GetEnumerator();
    }
}
