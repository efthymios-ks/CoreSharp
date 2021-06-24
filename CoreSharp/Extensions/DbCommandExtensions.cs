using System;
using System.Data.Common;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DbCommand extensions. 
    /// </summary>
    public static partial class DbCommandExtensions
    {
        /// <summary>
        /// Return a new instance of the provider's class that implements the DbParameter class. 
        /// </summary>
        public static DbParameter CreateParameter(this DbCommand command, string name, object value)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        /// <summary>
        /// Adds the specified DbParameter object to the DbParameterCollection. 
        /// </summary>
        public static DbParameter AddParameterWithValue(this DbCommand command, string name, object value)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            var parameter = command.CreateParameter(name, value);
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
