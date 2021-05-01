using System;
using System.Data.Common;
using System.Threading.Tasks;
using CoreSharp.Extensions;
using CoreSharp.Interfaces.Repositories;

namespace CoreSharp.Implementations.Repositories
{
    public abstract class DapperUnitOfWork : IUnitOfWork
    {
        //Properties
        protected DbConnection Connection { get; }
        protected DbTransaction Transaction { get; private set; }

        //Constructors
        public DapperUnitOfWork(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Transaction = connection?.OpenTransaction();
        }

        //Methods 
        private async Task ResetTransactionAsync()
        {
            await Transaction?.DisposeAsync().AsTask();
            Transaction = await Connection?.OpenTransactionAsync();
        }

        public virtual async Task CommitAsync()
        {
            await Transaction?.CommitAsync();
            await ResetTransactionAsync();
        }

        public virtual async Task RollbackAsync()
        {
            await Transaction?.RollbackAsync();
            await ResetTransactionAsync();
        }

        public virtual void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
