using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Models
{
    //TODO: Add unit tests 
    /// <summary>
    /// Simple IGrouping implementation. 
    /// </summary> 
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        //Fields
        private readonly IEnumerable<TElement> _source = Enumerable.Empty<TElement>();

        //Constructors 
        public Grouping(TKey key, IEnumerable<TElement> source)
        {
            Key = key;
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString();

        public TKey Key { get; }

        //Methods 
        public override string ToString() => $"{Key} ({this.Count()})";

        public virtual IEnumerator<TElement> GetEnumerator() => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
