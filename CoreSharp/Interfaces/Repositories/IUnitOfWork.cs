using System;
using System.Threading.Tasks;

namespace CoreSharp.Interfaces.Repositories
{
    [Obsolete]
    internal interface IUnitOfWork : IDisposable
    {
        //Metods 
        Task CommitAsync();

        Task RollbackAsync();
    }
}
