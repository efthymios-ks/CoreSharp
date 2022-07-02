using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Dummies.Services.Marked;

/// <summary>
/// Self match, but has <see cref="IgnoreServiceAttribute"/>.
/// Should ignore.
/// </summary>
[IgnoreService]
public class MarkedServiceWithIgnoreServiceAttributeOnItself : IScoped
{
}
