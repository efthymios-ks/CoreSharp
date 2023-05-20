using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Enumerables;

public class ShiftingStack<TElement> : IReadOnlyCollection<TElement>
{
    // Fields 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IList<TElement> _source = new List<TElement>();

    // Constructors
    public ShiftingStack(int maxCapacity)
        : this(maxCapacity, Enumerable.Empty<TElement>())
    {
    }

    public ShiftingStack(int maxCapacity, IEnumerable<TElement> source)
    {
        if (maxCapacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCapacity));
        }

        _ = source ?? throw new ArgumentNullException(nameof(source));

        MaxCapacity = maxCapacity;
        foreach (var item in source)
        {
            Push(item);
        }
    }

    // Properties
    /// <summary>
    /// Maximum number of items allowed in the <see cref="ShiftingStack{TElement}"/>.
    /// If max capacity is met, then bottom items are shifted out
    /// when new items are pushed on top.
    /// </summary>
    public int MaxCapacity { get; }

    /// <summary>
    /// Number of elements contained in the <see cref="ShiftingStack{TElement}"/>.
    /// </summary>
    public int Count
        => _source.Count;

    public bool HasItems
        => _source.Count > 0;

    public bool HasMetMaxCapacity
        => Count == MaxCapacity;

    // Methods
    /// <summary>
    /// Removes all items.
    /// </summary>
    public void Clear()
        => _source.Clear();

    /// <summary>
    /// Determines whether stack contains
    /// the provided item.
    /// </summary>
    public bool Contains(TElement item)
        => _source.Contains(item);

    /// <summary>
    /// Inserts an object at the top of the <see cref="ShiftingStack{TElement}"/>.
    /// </summary>
    public void Push(TElement item)
    {
        if (HasMetMaxCapacity)
        {
            _source.RemoveAt(0);
        }

        _source.Add(item);
    }

    /// <summary>
    /// Removes and returns the object at the top
    /// of the <see cref="ShiftingStack{TElement}"/>.
    /// </summary>
    public TElement Pop()
    {
        if (!HasItems)
        {
            throw new InvalidOperationException("Cannot pop on empty collection.");
        }

        var element = _source.Last();
        _source.Remove(element);
        return element;
    }

    /// <summary>
    /// Returns the object at the top of the
    /// <see cref="ShiftingStack{TElement}"/> without removing it.
    /// </summary>
    public TElement Peek()
    {
        if (!HasItems)
        {
            throw new InvalidOperationException("Cannot peek on empty collection.");
        }

        return _source.Last();
    }

    /// <summary>
    /// Returns a value that indicates whether there is an object
    /// at the top of the <see cref="ShiftingStack{TElement}"/>,
    /// and if one is present, copies it to the result parameter,
    /// and removes it from the <see cref="ShiftingStack{TElement}"/>
    /// </summary>
    public bool TryPop(out TElement element)
    {
        if (!HasItems)
        {
            element = default;
            return false;
        }

        element = Pop();
        return true;
    }

    /// <summary>
    /// Returns a value that indicates whether there is an object
    /// at the top of the <see cref="ShiftingStack{TItem}"/>,
    /// and if one is present, copies it to the result parameter.
    /// The object is not removed from the <see cref="ShiftingStack{TItem}"/>.
    /// </summary>
    public bool TryPeek(out TElement element)
    {
        if (!HasItems)
        {
            element = default;
            return false;
        }

        element = Peek();
        return true;
    }

    public IEnumerator<TElement> GetEnumerator()
        => _source.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
