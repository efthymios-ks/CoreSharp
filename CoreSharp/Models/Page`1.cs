using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Page<TEntity> : IEnumerable<TEntity>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEnumerable<TEntity> _items;

        //Constructors
        public Page(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable<TEntity> items)
        {
            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), $"{nameof(pageNumber)} has to be positive.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => ToString();
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }

        //Methods
        public override string ToString()
            => $"Page Number= {PageNumber}, Page Size={PageSize}";

        public IEnumerator<TEntity> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
