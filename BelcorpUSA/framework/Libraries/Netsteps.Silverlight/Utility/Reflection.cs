using System;
using System.Collections.Generic;
using System.Reflection;
using NetSteps.Silverlight.Base;

namespace NetSteps.Silverlight
{
    public class Reflection
    {
        private static ThreadSafeDictionary<Type, List<PropertyInfo>> ReflectionCache = new ThreadSafeDictionary<Type, List<PropertyInfo>>();

        public static List<PropertyInfo> FindClassProperties(Type objectType)
        {
            if (ReflectionCache.ContainsKey(objectType))
                return ReflectionCache[objectType];

            List<PropertyInfo> result = new List<PropertyInfo>(objectType.GetProperties());

            ReflectionCache.Add(objectType, result);

            return result;
        }

        public static List<PropertyInfo> FindClassProperties(Type objectType, List<string> propertiesToFind)
        {
            List<PropertyInfo> props = FindClassProperties(objectType);
            List<PropertyInfo> result = new List<PropertyInfo>();

            foreach (PropertyInfo pi in props)
            {
                foreach (string propertyToFind in propertiesToFind)
                {
                    if (pi.Name == propertyToFind)
                    {
                        result.Add(pi);
                        break;
                    }
                }
            }

            return result;
        }

        public static PropertyInfo FindClassProperty(Type objectType, string property)
        {
            List<PropertyInfo> result = FindClassProperties(objectType, new List<string>() { property });

            if (result.Count > 0)
                return result[0];
            else
                return null;
        }


        public static void CopyProperties<T>(T destination, T source)
        {
            var props = Reflection.FindClassProperties(typeof(T));
            foreach (PropertyInfo pi in props)
            {
                if (pi.CanRead && pi.CanWrite)
                {
                    pi.SetValue(destination, pi.GetValue(source, null), null);
                }
            }
        }

    }
}