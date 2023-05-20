using CoreSharp.DependencyInjection.ByInterface.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

internal interface ISingletonServiceWithContract
{
}

internal sealed class SingletonServiceWithContract :
    ISingletonServiceWithContract,
    ISingleton<ISingletonServiceWithContract>
{
}
