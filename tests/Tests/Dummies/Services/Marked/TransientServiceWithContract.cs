using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Dummies.Services.Marked;

internal interface ITransientServiceWithContract
{
}

internal class TransientServiceWithContract :
    ITransientServiceWithContract,
    ITransient<ITransientServiceWithContract>
{
}
