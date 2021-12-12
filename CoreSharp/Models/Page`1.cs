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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _pageNumber;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _pageSize;

        //Constructors
        public Page(IEnumerable<TEntity> items)
            => _items = items ?? throw new ArgumentNullException(nameof(items));

        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => ToString();

        public int PageNumber
        {
            get => _pageNumber;
            init
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(PageNumber), $"{nameof(PageNumber)} has to be positive.");
                _pageNumber = value;
            }
        }

        public int PageSize
        {
            get => _pageSize;
            init
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(PageSize), $"{nameof(PageSize)} has to be positive and non-zero.");
                _pageSize = value;
            }
        }

        public int TotalItems { get; init; }

        public int TotalPages { get; init; }

        //Methods
        public override string ToString()
            => $"Page Number= {PageNumber}, Page Size={PageSize}";

        public IEnumerator<TEntity> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
