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
        private void ResetTransaction()
        {
            Transaction?.Dispose();
            Transaction = Connection?.BeginTransaction();
        }

        public Task CommitAsync()
        {
            Transaction?.Commit();
            ResetTransaction();

            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            Transaction?.Rollback();
            ResetTransaction();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
