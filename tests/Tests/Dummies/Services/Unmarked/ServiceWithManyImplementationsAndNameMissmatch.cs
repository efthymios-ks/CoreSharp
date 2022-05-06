namespace Tests.Dummies.Services.Unmarked
{
    public interface IServiceWithManyImplementationsAndNameMissmatch
    {
    }

    /// <summary>
    /// Name missmatch.
    /// Should not register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMissmatch1 : IServiceWithManyImplementationsAndNameMissmatch
    {
    }

    /// <summary>
    /// Name missmatch.
    /// Should not register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMissmatch2 : IServiceWithManyImplementationsAndNameMissmatch
    {
    }

    /// <summary>
    /// Name missmatch.
    /// Should not register.
    /// </summary>
    public class ServiceWithManyImplementationsAndNameMissmatch3 : IServiceWithManyImplementationsAndNameMissmatch
    {
    }
}
