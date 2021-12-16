using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Models.Pages
{
    public class Page<TEntity> : Page, IEnumerable<TEntity>
    {
        //Constructors
        public Page(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable<TEntity> items)
            : base(pageNumber, pageSize, totalItems, totalPages, items)
        {
        }

        //Methods
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
            => Items.Cast<TEntity>().GetEnumerator();
    }
}
