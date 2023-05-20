using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.ByInterface.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

public interface IMarkedServiceWithIgnoreServiceAttributeOnImplementation
{
}

/// <summary>
/// Matches, but has <see cref="IgnoreServiceAttribute"/>.
/// Should ignore.
/// </summary>
[IgnoreService]
public class MarkedServiceWithIgnoreServiceAttributeOnImplementation :
    IMarkedServiceWithIgnoreServiceAttributeOnImplementation,
    IScoped<IMarkedServiceWithIgnoreServiceAttributeOnImplementation>
{
}
