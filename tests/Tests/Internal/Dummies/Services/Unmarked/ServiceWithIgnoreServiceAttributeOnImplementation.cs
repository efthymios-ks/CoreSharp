using CoreSharp.DependencyInjection.Attributes;

namespace Tests.Internal.Dummies.Services.Unmarked;

public interface IServiceWithIgnoreServiceAttributeOnImplementation
{
}

/// <summary>
/// Matches, but has <see cref="IgnoreServiceAttribute"/>.
/// Should ignore.
/// </summary>
[IgnoreService]
public class ServiceWithIgnoreServiceAttributeOnImplementation : IServiceWithIgnoreServiceAttributeOnImplementation
{
}
