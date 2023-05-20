using CoreSharp.DependencyInjection.ByInterface.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

internal interface IScopedServiceWithContract
{
}

internal sealed class ScopedServiceWithContract :
    IScopedServiceWithContract,
    IScoped<IScopedServiceWithContract>
{
}
