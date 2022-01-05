using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.Extensions
{

    /// <summary>
    /// <see cref="PropertyInfo"/> extensions.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <inheritdoc cref="PropertyInfo.GetValue(object?)" />
        public static TValue GetValue<TValue>(this PropertyInfo property, object item)
        {
            _ = property ?? throw new ArgumentNullException(nameof(property));

            return (TValue)property.GetValue(item);
        }
    }
}
