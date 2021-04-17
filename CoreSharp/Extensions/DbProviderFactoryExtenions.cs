using System;
using System.Data.Common;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DbProviderFactory extenions
    /// </summary>
    public static partial class DbProviderFactoryExtenions
    {
        /// <summary>
        /// Return a new instance of the provider's class that implements the DbParameter class. 
        /// </summary>
        public static DbParameter CreateParameter(this DbProviderFactory factory, string parameterName, object parameterValue)
        {
            factory = factory ?? throw new ArgumentNullException(nameof(factory));

            var parameter = factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
    }
}
