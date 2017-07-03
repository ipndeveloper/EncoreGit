using System;

namespace NetSteps.Data.Entities.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull<T>(this T tObject) where T : class
        {
            return tObject != null;
        }

        public static bool IsNull<T>(this T tObject) where T : class
        {
            return tObject == null;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ms366789.aspx
        /// NOTE: Do not use GetType() with this method.
        /// GetType() returns the underlying type, not the Nullable type, so this will always return false.
        /// </summary>
        public static bool IsNullableType<T>(this T tObject)
        {
            Type type = typeof(T);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

    }
}
