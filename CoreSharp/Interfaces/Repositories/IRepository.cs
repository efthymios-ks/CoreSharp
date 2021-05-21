using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreSharp.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //Methods
        Task<TEntity> GetAsync(params object[] key);

        Task<IEnumerable<TEntity>> GetAsync();

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
    }
}
