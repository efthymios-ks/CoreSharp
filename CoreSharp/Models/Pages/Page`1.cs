using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Models.Pages
{
    public class Page<TEntity> : Page
    {
        //Constructors
        public Page(int pageNumber, int pageSize, int totalItems, int totalPages, IEnumerable<TEntity> items)
            : base(pageNumber, pageSize, totalItems, totalPages, items)
        {
        }

        //Properties
        public new IEnumerable<TEntity> Items => (this as Page).Items.Cast<TEntity>();
    }
}
