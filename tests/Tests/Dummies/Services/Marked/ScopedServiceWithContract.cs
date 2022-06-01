using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Dummies.Services.Marked
{
    internal interface IScopedServiceWithContract
    {
    }

    internal class ScopedServiceWithContract :
        IScopedServiceWithContract,
        IScoped<IScopedServiceWithContract>
    {
    }
}
