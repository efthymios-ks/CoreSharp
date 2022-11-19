namespace Tests.Internal.Dummies.Services.Unmarked;

public interface IServiceWithSingleImplementationAndNameMissmatch
{
}

/// <summary>
/// Name missmatch and single implementation.
/// Should register.
/// </summary>
public class ServiceWithSingleImplementationAndNameMissmatch1 : IServiceWithSingleImplementationAndNameMissmatch
{
}
