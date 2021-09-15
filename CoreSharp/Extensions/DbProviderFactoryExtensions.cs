﻿using System;
using System.Data.Common;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DbProviderFactory extensions
    /// </summary>
    public static class DbProviderFactoryExtensions
    {
        /// <summary>
        /// Return a new instance of the provider's class that implements the DbParameter class.
        /// </summary>
        public static DbParameter CreateParameter(this DbProviderFactory factory, string parameterName, object parameterValue)
        {
            _ = factory ?? throw new ArgumentNullException(nameof(factory));

            var parameter = factory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
    }
}