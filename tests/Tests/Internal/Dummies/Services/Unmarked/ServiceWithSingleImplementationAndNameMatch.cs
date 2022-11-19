namespace Tests.Internal.Dummies.Services.Unmarked;

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
