using CoreSharp.DependencyInjection.Attributes;

namespace Tests.Dummies.Services.Unmarked;

public interface IServiceWithSingleImplementationAndNameMatch
{
}

/// <summary>
/// Name missmatch and single implementation.
/// Should register.
/// </summary>
public class ServiceWithSingleImplementationAndNameMatch : IServiceWithSingleImplementationAndNameMatch
{
}
