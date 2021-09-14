using System;
using System.Threading.Tasks;

namespace CoreSharp.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //Methods 
        Task CommitAsync();

        Task RollbackAsync();
    }
}
