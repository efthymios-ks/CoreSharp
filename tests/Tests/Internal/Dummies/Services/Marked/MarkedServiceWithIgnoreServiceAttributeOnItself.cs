using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.ByInterface.Interfaces;

namespace Tests.Internal.Dummies.Services.Marked;

/// <summary>
/// Self match, but has <see cref="IgnoreServiceAttribute"/>.
/// Should ignore.
/// </summary>
[IgnoreService]
public class MarkedServiceWithIgnoreServiceAttributeOnItself : IScoped
{
}
