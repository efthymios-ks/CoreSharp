using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

internal interface ITransientServiceWithContract
{
}

internal sealed class TransientServiceWithContract :
    ITransientServiceWithContract,
    ITransient<ITransientServiceWithContract>
{
}
