using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Models.Pages
{
    public class Page<TElement> : Page
    {
        //Constructors
        public Page(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable<TElement> source)
            : base(pageNumber, pageSize, totalItems, totalPages, source)
        {
        }

        //Properties
        public new IEnumerable<TElement> Items
            => (this as Page).Items.Cast<TElement>();
    }
}
