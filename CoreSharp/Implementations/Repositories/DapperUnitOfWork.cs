using System;
using System.Data.Common;
using System.Threading.Tasks;
using CoreSharp.Extensions;
using CoreSharp.Interfaces.Repositories;

namespace CoreSharp.Implementations.Repositories
{
    [Obsolete]
    internal abstract class DapperUnitOfWork : IUnitOfWork
    {
        //Properties
        protected DbConnection Connection { get; }

        protected DbTransaction Transaction { get; private set; }

        //Constructors
        protected DapperUnitOfWork(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Transaction = connection?.OpenTransaction();
        }

        //Methods 
        private async Task ResetTransactionAsync()
        {
            await (Transaction?.DisposeAsync().AsTask()).ConfigureAwait(false);
            Transaction = await (Connection?.OpenTransactionAsync()).ConfigureAwait(false);
        }

        public virtual async Task CommitAsync()
        {
            await (Transaction?.CommitAsync()).ConfigureAwait(false);
            await ResetTransactionAsync().ConfigureAwait(false);
        }

        public virtual async Task RollbackAsync()
        {
            await (Transaction?.RollbackAsync()).ConfigureAwait(false);
            await ResetTransactionAsync().ConfigureAwait(false);
        }

        public virtual void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
