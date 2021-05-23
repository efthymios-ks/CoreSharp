using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSharp.Interfaces.EntityFramework;
using CoreSharp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        //Fields
        private readonly IQueryable<TEntity> table;

        //Constructors
        public EfRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            table = Context.Set<TEntity>();
        }

        //Properties 
        protected DbContext Context { get; }

        //Methods 
        private IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation)
        {
            var query = table;

            if (navigation != null)
                query = navigation(query);

            if (filter != null)
                query = query.Where(filter);

            return query;
        }

        public async virtual Task<TEntity> GetAsync(object key, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            var query = BuildQuery(null, navigation);
            return await query.SingleOrDefaultAsync(i => i.Id.Equals(key));
        }

        public async virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            var query = BuildQuery(filter, navigation);
            return await query.ToArrayAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.CreatedDate = DateTime.UtcNow;
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.ModifiedDate = DateTime.UtcNow;
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            Context.Set<TEntity>().Remove(entity);

            return Task.CompletedTask;
        }
    }
}
