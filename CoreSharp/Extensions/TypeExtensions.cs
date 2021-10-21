using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Type"/> extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Check if given <see cref="Type"/> is numeric.
        /// </summary>
        public static bool IsNumeric(this Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                case TypeCode.Object:
                    {
                        var baseType = Nullable.GetUnderlyingType(type);
                        return baseType?.IsNumeric() ?? false;
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if given <see cref="Type"/> is <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
        /// </summary>
        public static bool IsDate(this Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DateTime:
                case TypeCode.Object when type == typeof(DateTimeOffset):
                    return true;
                case TypeCode.Object:
                    {
                        var baseType = Nullable.GetUnderlyingType(type);
                        return baseType?.IsDate() ?? false;
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks for
        /// <see cref="Type.IsPrimitive"/>,
        /// <see cref="string"/>,
        /// <see cref="decimal"/>,
        /// <see cref="DateTime"/>,
        /// <see cref="DateTimeOffset"/>,
        /// <see cref="Guid"/>.
        /// </summary>
        public static bool IsExtendedPrimitive(this Type type)
        {
            var baseType = Nullable.GetUnderlyingType(type) ?? type;

            if (baseType.IsPrimitive)
            {
                return true;
            }
            else
            {
                var allowedTypes = new[]
                {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(Guid)
                };
                return allowedTypes.Any(t => Equals(t, type));
            }
        }

        //TODO: Add unit tests
        /// <summary>
        /// If <see cref="Nullable{T}"/> return <see cref="Nullable.GetUnderlyingType(Type)"/>, else the provided <see cref="Type"/> itself.
        /// </summary>
        public static Type GetNullableBaseType(this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        /// <summary>
        /// Get list of specific <see cref="Attribute"/>.
        /// </summary>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Type type) where TAttribute : Attribute
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttributes(typeof(TAttribute), true)?.Cast<TAttribute>();
        }

        /// <summary>
        /// Get specific <see cref="Attribute"/>.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
            => type.GetAttributes<TAttribute>()?.FirstOrDefault();
    }
}
