using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="MemberInfo"/> extensions.
/// </summary>
public static class MemberInfoExtensions
{
    /// <inheritdoc cref="TypeExtensions.GetAttributes{TAttribute}(Type)" />
    public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this MemberInfo field)
        where TAttribute : Attribute
    {
        _ = field ?? throw new ArgumentNullException(nameof(field));

        return field.GetCustomAttributes(typeof(TAttribute), true)?.Cast<TAttribute>();
    }

    /// <inheritdoc cref="TypeExtensions.GetAttribute{TAttribute}(Type)" />
    public static TAttribute GetAttribute<TAttribute>(this MemberInfo field)
        where TAttribute : Attribute
        => field.GetAttributes<TAttribute>()?.FirstOrDefault();
}
