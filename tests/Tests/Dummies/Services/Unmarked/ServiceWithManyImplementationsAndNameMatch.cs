namespace Tests.Dummies.Services.Unmarked
{
    public interface IServiceWithManyImplementationsAndNameMatch
    {
    }

    /// <summary>
    /// Name match.
    /// Should register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMatch : IServiceWithManyImplementationsAndNameMatch
    {
    }

    /// <summary>
    /// Name missmatch.
    /// Should not register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMatch2 : IServiceWithManyImplementationsAndNameMatch
    {
    }

    /// <summary>
    /// Name missmatch.
    /// Should not register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMatch3 : IServiceWithManyImplementationsAndNameMatch
    {
    }
}
