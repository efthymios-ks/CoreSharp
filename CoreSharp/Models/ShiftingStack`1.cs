using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Models
{
    public class ShiftingStack<TItem> : IReadOnlyCollection<TItem>
    {
        //Fields 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<TItem> _source = new List<TItem>();

        //Constructors
        public ShiftingStack(int maxCapacity)
            : this(maxCapacity, Enumerable.Empty<TItem>())
        {
        }

        public ShiftingStack(int maxCapacity, IEnumerable<TItem> source)
        {
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            MaxCapacity = maxCapacity;
            foreach (var item in source)
                Push(item);
        }

        //Properties
        /// <summary>
        /// Maximum number of items allowed in the <see cref="ShiftingStack{TItem}"/>.
        /// If max capacity is met, then bottom items are shifted out
        /// when new items are pushed on top.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Number of elements contained in the <see cref="ShiftingStack{TItem}"/>.
        /// </summary>
        public int Count
            => _source.Count;

        public bool HasItems
            => _source.Any();

        public bool HasMetMaxCapacity
            => Count == MaxCapacity;

        //Methods
        /// <summary>
        /// Removes all items.
        /// </summary>
        public void Clear()
            => _source.Clear();

        /// <summary>
        /// Determines whether stack contains
        /// the provided item.
        /// </summary>
        public bool Contains(TItem item)
            => _source.Contains(item);

        /// <summary>
        /// Inserts an object at the top of the <see cref="ShiftingStack{TItem}"/>.
        /// </summary>
        public void Push(TItem item)
        {
            if (HasMetMaxCapacity)
                _source.RemoveAt(0);

            _source.Add(item);
        }

        /// <summary>
        /// Removes and returns the object at the top
        /// of the <see cref="ShiftingStack{TItem}"/>.
        /// </summary>
        public TItem Pop()
        {
            if (!HasItems)
                throw new InvalidOperationException("Cannot pop on empty collection.");

            var item = _source.Last();
            _source.Remove(item);
            return item;
        }

        /// <summary>
        /// Returns the object at the top of the
        /// <see cref="ShiftingStack{TItem}"/> without removing it.
        /// </summary>
        public TItem Peek()
        {
            if (!HasItems)
                throw new InvalidOperationException("Cannot peek on empty collection.");

            return _source.Last();
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object
        /// at the top of the <see cref="ShiftingStack{TItem}"/>,
        /// and if one is present, copies it to the result parameter,
        /// and removes it from the <see cref="ShiftingStack{TItem}"/>
        /// </summary>
        public bool TryPop(out TItem item)
        {
            if (!HasItems)
            {
                item = default;
                return false;
            }

            item = Pop();
            return true;
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object
        /// at the top of the <see cref="ShiftingStack{TItem}"/>,
        /// and if one is present, copies it to the result parameter.
        /// The object is not removed from the <see cref="ShiftingStack{TItem}"/>.
        /// </summary>
        public bool TryPeek(out TItem item)
        {
            if (!HasItems)
            {
                item = default;
                return false;
            }

            item = Peek();
            return true;
        }

        public IEnumerator<TItem> GetEnumerator()
            => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
