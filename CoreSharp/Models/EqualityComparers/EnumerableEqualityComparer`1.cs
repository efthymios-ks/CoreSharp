using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Models.EqualityComparers
{
    public class EnumerableEqualityComparer<TEntity> : IEqualityComparer<IEnumerable<TEntity>>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEqualityComparer<TEntity> _entityComparer;

        //Constructors 
        public EnumerableEqualityComparer() : this(EqualityComparer<TEntity>.Default)
        {
        }

        public EnumerableEqualityComparer(IEqualityComparer<TEntity> entityComparer = null)
            => _entityComparer = entityComparer ?? throw new ArgumentNullException(nameof(entityComparer));

        //Methods
        public bool Equals(IEnumerable<TEntity> left, IEnumerable<TEntity> right)
        {
            if (left is null && right is not null)
                return false;
            else if (left is not null && right is null)
                return false;
            else if (left is null && right is null)
                return true;

            //Different count 
            if (left.Count() != right.Count())
                return false;
            //Left contains more items
            else if (left.Except(right, _entityComparer).Any())
                return false;
            //Right contains more items
            else if (right.Except(left, _entityComparer).Any())
                return false;

            return true;
        }

        public int GetHashCode(IEnumerable<TEntity> source)
        {
            if (source is null)
                return _entityComparer.GetHashCode(default);

            var hash = new HashCode();
            foreach (var entity in source)
                hash.Add(entity, _entityComparer);
            return hash.ToHashCode();
        }
    }
}
