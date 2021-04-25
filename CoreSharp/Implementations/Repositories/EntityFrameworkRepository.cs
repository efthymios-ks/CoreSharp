using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSharp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //Properties
        protected abstract IQueryable<TEntity> Query { get; }

        protected DbContext Context { get; }

        //Constructors
        public EntityFrameworkRepository(DbContext context)
        {
            Context = context;
        }

        //Methods 
        public virtual async Task<TEntity> GetAsync(params object[] key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            return await Context.Set<TEntity>().FindAsync(key);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await Query.ToArrayAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Predicate<TEntity> predicate)
        {
            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            return await Query.Where(i => predicate(i)).ToArrayAsync();
        }

        public virtual Task AddAsync(params TEntity[] entities)
        {
            entities = entities ?? throw new ArgumentNullException(nameof(entities));

            if (entities.Length == 1)
                Context.Set<TEntity>().Add(entities[0]);
            else
                Context.Set<TEntity>().AddRange(entities);

            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(params TEntity[] entities)
        {
            entities = entities ?? throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(params TEntity[] entities)
        {
            entities = entities ?? throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Context.Set<TEntity>().Remove(entity);

            return Task.CompletedTask;
        }
    }
}
