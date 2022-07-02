using System;
using System.Data.Common;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="DbProviderFactory"/> extensions.
/// </summary>
public static class DbProviderFactoryExtensions
{
    /// <inheritdoc cref="DbProviderFactory.CreateParameter"/>
    public static DbParameter CreateParameter(this DbProviderFactory factory, string parameterName, object parameterValue)
    {
        _ = factory ?? throw new ArgumentNullException(nameof(factory));

        var parameter = factory.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = parameterValue;
        return parameter;
    }
}
