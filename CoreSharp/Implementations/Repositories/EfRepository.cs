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
        private readonly DbSet<TEntity> table;

        //Constructors
        public EfRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            table = Context.Set<TEntity>();
        }

        //Properties 
        protected DbContext Context { get; }

        //Methods 
        public async virtual Task<TEntity> GetAsync(object key, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            var entities = await GetAsync(i => i.Id.Equals(key), navigation);
            return entities.SingleOrDefault();
        }

        public async virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> navigation = null)
        {
            var query = table.AsQueryable();
            if (navigation != null)
                query = navigation(query);
            if (filter != null)
                query = query.Where(filter);

            return await query.ToArrayAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.DateCreated = DateTime.UtcNow;
            await table.AddAsync(entity);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            entity.DateModified = DateTime.UtcNow;
            table.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            table.Remove(entity);

            return Task.CompletedTask;
        }
    }
}
