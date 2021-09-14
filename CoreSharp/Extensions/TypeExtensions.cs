using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Check if given type is numeric.
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
        /// Check if given type is DateTime(Offset).
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

        //TODO: Add unit tests
        /// <summary>
        /// If Nullable`T return base type, else the same type.
        /// </summary>
        public static Type GetNullableBaseType(this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;
    }
}
