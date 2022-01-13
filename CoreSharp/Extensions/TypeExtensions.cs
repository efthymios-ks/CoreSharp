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
                    var baseType = Nullable.GetUnderlyingType(type);
                    return baseType?.IsNumeric() ?? false;
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
        /// <see cref="Guid"/>,
        /// <see cref="TimeSpan"/>,
        /// <see cref="DateTime"/>,
        /// <see cref="DateTimeOffset"/>,
        /// <see cref="Enum"/>.
        /// </summary>
        public static bool IsPrimitiveExtended(this Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            //Nullable extraction
            var baseType = Nullable.GetUnderlyingType(type) ?? type;

            //Enum extraction 
            if (baseType.IsEnum)
                baseType = Enum.GetUnderlyingType(baseType);

            //Base type check 
            if (baseType.IsPrimitive)
                return true;

            //Additional type check 
            var additionalTypes = new[]
            {
                typeof(string),
                typeof(decimal),
                typeof(Guid),
                typeof(TimeSpan),
                typeof(DateTime),
                typeof(DateTimeOffset)
            };
            return additionalTypes.Any(t => t == baseType);
        }

        /// <summary>
        /// If <see cref="Nullable{T}"/> return <see cref="Nullable.GetUnderlyingType(Type)"/>, else the provided <see cref="Type"/> itself.
        /// </summary>
        public static Type GetNullableBaseType(this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        /// <summary>
        /// Returns a <see cref="Type"/> that represents a generic type definition
        /// from which the current generic type can be constructed.<br/>
        /// If <see cref="Type.IsGenericType"/> is <see langword="false"/>,
        /// the provided <see cref="Type"/> itself is returned.
        /// </summary>
        public static Type GetGenericTypeBase(this Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

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

        /// <summary>
        /// Get top-level interfaces excluding nested ones.
        /// </summary>
        public static IEnumerable<Type> GetDirectInterfaces(this Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            var topLevelInterfaces = new HashSet<Type>();
            var nestedInterfaces = new HashSet<Type>();

            foreach (var topLevelInterface in type.GetInterfaces())
            {
                topLevelInterfaces.Add(topLevelInterface);

                foreach (var nestedInterface in topLevelInterface.GetDirectInterfaces())
                    nestedInterfaces.Add(nestedInterface);
            }

            return topLevelInterfaces.Except(nestedInterfaces);
        }

        /// <inheritdoc cref="GetDefault(Type)"/>
        public static TValue GetDefault<TValue>(this Type type)
            => (TValue)type.GetDefault();

        /// <summary>
        /// Runtime equivalent of default(T).
        /// </summary>
        public static object GetDefault(this Type type)
            => type.IsValueType ? Activator.CreateInstance(type) : null;

        /// <summary>
        /// Determines whether the current <see cref="Type"/>
        /// derives from the specified base <see cref="Type"/>.
        /// </summary>
        public static bool Implements(this Type type, Type baseType, bool useGenericBaseType = false)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            _ = baseType ?? throw new ArgumentNullException(nameof(baseType));

            if (useGenericBaseType)
            {
                if (type.IsGenericType)
                    type = type.GetGenericTypeDefinition();
                if (baseType.IsGenericType)
                    baseType = baseType.GetGenericTypeDefinition();
            }

            //Same type 
            if (type == baseType)
                return true;

            //Implement class 
            else if (type.IsSubclassOf(baseType))
                return true;

            //Implement interface 
            else if (baseType.IsInterface && type.GetInterface(baseType.FullName) is not null)
                return true;

            //No relation
            else
                return false;
        }
    }
}
