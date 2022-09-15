using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.EqualityComparers;

public class EnumerableEqualityComparer<TEntity> : IEqualityComparer<IEnumerable<TEntity>>
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEqualityComparer<TEntity> _entityComparer;

    // Constructors 
    public EnumerableEqualityComparer()
        : this(EqualityComparer<TEntity>.Default)
    {
    }

    public EnumerableEqualityComparer(IEqualityComparer<TEntity> entityComparer)
        => _entityComparer = entityComparer ?? throw new ArgumentNullException(nameof(entityComparer));

    // Methods
    public bool Equals(IEnumerable<TEntity> x, IEnumerable<TEntity> y)
    {
        // Same reference 
        if (x == y)
            return true;
        // Null 
        else if (x is null || y is null)
            return false;

        // Different count 
        if (x.Count() != y.Count())
            return false;
        // Left contains more items
        else if (x.Except(y, _entityComparer).Any())
            return false;
        // Right contains more items
        else if (y.Except(x, _entityComparer).Any())
            return false;

        return true;
    }

    public int GetHashCode(IEnumerable<TEntity> obj)
    {
        if (obj is null)
            return _entityComparer.GetHashCode(default);

        var hash = new HashCode();
        foreach (var entity in obj)
            hash.Add(entity, _entityComparer);
        return hash.ToHashCode();
    }
}
