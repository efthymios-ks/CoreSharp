using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CoreSharp.Interfaces.Repositories;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : class
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
        public abstract Task<TEntity> GetAsync(params object[] key);

        public abstract Task<IEnumerable<TEntity>> GetAsync();

        public async virtual Task<IEnumerable<TEntity>> GetAsync(Predicate<TEntity> predicate)
        {
            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            return (await GetAsync()).Where(i => predicate(i));
        }

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
