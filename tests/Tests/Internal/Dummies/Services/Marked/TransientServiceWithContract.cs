using CoreSharp.DependencyInjection.ByInterface.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

internal interface ITransientServiceWithContract
{
}

internal sealed class TransientServiceWithContract :
    ITransientServiceWithContract,
    ITransient<ITransientServiceWithContract>
{
}
