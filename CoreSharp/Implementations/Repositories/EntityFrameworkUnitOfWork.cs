using System;
using System.Linq;
using System.Threading.Tasks;
using CoreSharp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        //Properties
        protected DbContext Context { get; }

        //Constructors
        public EntityFrameworkUnitOfWork(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //Methods   
        public virtual async Task CommitAsync()
        {
            await Context?.SaveChangesAsync();
        }

        public virtual async Task RollbackAsync()
        {
            var entries = Context?.ChangeTracker?.Entries()?.Where(e => e.State != EntityState.Unchanged);
            if (entries == null)
                return;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public virtual void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
