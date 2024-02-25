using System.Diagnostics.CodeAnalysis;

namespace CoreSharp.DependencyInjection.ByInterface.Interfaces;

[SuppressMessage(
    "Major Code Smell", "S2326:Unused type parameters should be removed",
    Justification = "<Pending>")]
public interface ISingleton<TContract> : ISingleton
{
}
