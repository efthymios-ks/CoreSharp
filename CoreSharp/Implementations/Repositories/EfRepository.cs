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
        //Constructors
        public EfRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Table = Context.Set<TEntity>();
        }

        //Properties 
        protected DbContext Context { get; }

        protected DbSet<TEntity> Table { get; }

        //Methods 
        public async virtual Task<TEntity> GetAsync(object key, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            var entities = await GetAsync(i => i.Id.Equals(key), navigation);
            return entities.SingleOrDefault();
        }

        public async virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            var query = Table.AsQueryable();
            if (navigation is not null)
                query = navigation(query);
            if (filter is not null)
                query = query.Where(filter);

            return await query.ToArrayAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.DateCreated = DateTime.UtcNow;
            await Table.AddAsync(entity);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.DateModified = DateTime.UtcNow;
            Table.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            Table.Remove(entity);

            return Task.CompletedTask;
        }
    }
}
