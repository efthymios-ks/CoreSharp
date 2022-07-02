using CoreSharp.DependencyInjection.Attributes;
using CoreSharp.DependencyInjection.Interfaces;

namespace Tests.Dummies.Services.Marked;

[IgnoreService]
public interface IMarkedServiceWithIgnoreServiceAttributeOnContract
{
}

/// <summary>
/// Matches, but has <see cref="IgnoreServiceAttribute"/>.
/// Should ignore.
/// </summary>
public class MarkedServiceWithIgnoreServiceAttributeOnContract :
    IMarkedServiceWithIgnoreServiceAttributeOnContract,
    IScoped<IMarkedServiceWithIgnoreServiceAttributeOnContract>
{
}
