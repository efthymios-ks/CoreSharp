using Microsoft.Extensions.DependencyInjection;
using System;
namespace CoreSharp.DependencyInjection.Attributes;

/// <summary>
/// Mark either interfaces (contracts) or classes (implementations) that need to be ignored by
/// <see cref="ByReflection.Extensions.IServiceCollectionExtensions.AddServices(IServiceCollection)"/> or
/// <see cref="ByInterface.Extensions.IServiceCollectionExtensions.AddMarkedServices(IServiceCollection)"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public sealed class IgnoreServiceAttribute : Attribute
{
}
