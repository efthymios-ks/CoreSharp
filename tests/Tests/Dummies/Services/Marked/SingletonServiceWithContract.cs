using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Dummies.Services.Marked
{
    internal interface ISingletonServiceWithContract
    {
    }

    internal class SingletonServiceWithContract : ISingletonServiceWithContract, ISingleton<ISingletonServiceWithContract>
    {
    }
}
