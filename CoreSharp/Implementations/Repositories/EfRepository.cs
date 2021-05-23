﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreSharp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreSharp.Implementations.Repositories
{
    [Obsolete]
    internal abstract class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //Constructors
        public EfRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //Properties 
        protected DbContext Context { get; }

        protected abstract IQueryable<TEntity> Query { get; }

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

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            return await Query.Where(predicate).ToArrayAsync();
        }

        public virtual Task AddAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

            Context.Set<TEntity>().Add(entity);

            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));

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
