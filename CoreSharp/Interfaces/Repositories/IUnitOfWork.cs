using System;
using System.Threading.Tasks;

namespace CoreSharp.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //Metods 
        Task CommitAsync();
        Task RollbackAsync();
    }
}
