using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSharp.Interfaces.EntityFramework;
using CoreSharp.Interfaces.Repositories;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        //Constructors 
        public DapperRepository(DbTransaction transaction)
        {
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        //Properties 
        protected DbConnection Connection => Transaction?.Connection;

        protected DbTransaction Transaction { get; }

        //Methods 
        public abstract Task<TEntity> GetAsync(object key, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null);

        public abstract Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null);

        /// <example>
        /// --Use OUTPUT to get auto-identity newly added Id 
        /// INSERT INTO Table 
        /// (Columns) 
        /// OUTPUT INSERTED.ColumnId 
        /// VALUES 
        /// (@ColumnValues) 
        /// </example> 
        public abstract Task AddAsync(TEntity entity);

        public abstract Task UpdateAsync(TEntity entity);

        public abstract Task RemoveAsync(TEntity entity);

    }
}
