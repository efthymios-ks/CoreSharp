using System;
using System.Data.Common;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="DbCommand"/> extensions.
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <inheritdoc cref="DbCommand.CreateDbParameter"/>
        public static DbParameter CreateParameter(this DbCommand command, string name, object value)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        /// <inheritdoc cref="DbParameterCollection.Add(object)"/>
        public static DbParameter AddParameterWithValue(this DbCommand command, string name, object value)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            var parameter = command.CreateParameter(name, value);
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
