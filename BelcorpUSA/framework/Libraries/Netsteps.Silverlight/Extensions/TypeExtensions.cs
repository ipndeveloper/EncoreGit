using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNumeric(this Type type)
        {
            type = GetNonNullableType(type);

            if (type != null && !type.IsEnum)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
            }

            return false;
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (IsNullableType(type))
                return Nullable.GetUnderlyingType(type);

            return type;
        }

        public static bool IsNullableType(this Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
