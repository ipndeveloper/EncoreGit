using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Common.Reflection
{
    public class Reflection
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyInfo>> _reflectionPropertyCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyInfo>>();
        private static readonly ConcurrentDictionary<Type, List<MethodInfo>> _reflectionMethodCache = new ConcurrentDictionary<Type, List<MethodInfo>>();
        private static readonly ConcurrentDictionary<Type[], MethodInfo> _reflectionGenericMethodCache = new ConcurrentDictionary<Type[], MethodInfo>();
        private static readonly ConcurrentDictionary<MethodInfo, ParameterInfo[]> _reflectionMethodParameterCache = new ConcurrentDictionary<MethodInfo, ParameterInfo[]>();
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type[], ConstructorInfo>> _reflectionConstructorCache = new ConcurrentDictionary<Type, ConcurrentDictionary<Type[], ConstructorInfo>>();



        private static ConcurrentDictionary<string, PropertyInfo> GetOrAddPropertiesToCache(Type t)
        {
            return _reflectionPropertyCache.GetOrAdd(t, key => new ConcurrentDictionary<string, PropertyInfo>(key.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name, p => p)));
        }

        private static List<MethodInfo> GetOrAddMethodInfoToCache(Type t)
        {
            return _reflectionMethodCache.GetOrAdd(t, key => new List<MethodInfo>(key.GetMethods()));
        }

        public static List<PropertyInfo> FindClassProperties(Type t)
        {
            return GetOrAddPropertiesToCache(t).Values.ToList();
        }

        public static PropertyInfo FindClassProperty(Type t, string property)
        {
            return GetOrAddPropertiesToCache(t).Values.FirstOrDefault(p => p.Name == property);
        }

        public static IEnumerable<PropertyInfo> FindClassPropertiesWithAttributeType(Type t, Type tAttr)
        {
            return FindClassProperties(t).Where(p => Attribute.IsDefined(p, tAttr));
        }

        public static IEnumerable<PropertyInfo> FindClassProperties(Type t, List<string> propertiesToFind)
        {
            return GetOrAddPropertiesToCache(t).Where(p => propertiesToFind.Contains(p.Key)).Select(p => p.Value);
        }

        /// <summary>
        /// Finds a property (can optionally search child objects for the property as well, i.e. Order.OrderType.Name)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyKeys"></param>
        /// <param name="actionOnEachProperty"></param>
        /// <returns></returns>
        public static bool PropertyExists(Type t, string propertyKeys, Action<Type, PropertyInfo> actionOnEachProperty = null)
        {
            string[] propertyKeysSplit = propertyKeys.Split('.');
            bool found = true;
            foreach (string propertyKey in propertyKeysSplit)
            {
                var properties = GetOrAddPropertiesToCache(t);

                PropertyInfo property;
                if (!properties.TryGetValue(propertyKey, out property))
                {
                    found = false;
                    break;
                }

                if (actionOnEachProperty != null)
                    actionOnEachProperty(t, property);

                t = property.PropertyType;
            }
            return found;
        }

        public static List<MethodInfo> FindClassMethods(Type objectType)
        {
            return GetOrAddMethodInfoToCache(objectType);
        }

        public static IEnumerable<MethodInfo> FindClassMethodsWithAttributeType(Type t, Type tAttr)
        {
            return GetOrAddMethodInfoToCache(t).Where(p => Attribute.GetCustomAttribute(p, tAttr) != null);
        }

        public static IEnumerable<MethodInfo> FindClassMethods(Type t, List<string> methodsToFind)
        {
            return GetOrAddMethodInfoToCache(t).Where(m => methodsToFind.Contains(m.Name));
        }

        public static IEnumerable<MethodInfo> FindClassMethodsByName(Type t, string methodName)
        {
            return GetOrAddMethodInfoToCache(t).Where(m => m.Name == methodName);
        }

        public static MethodInfo FindClassMethod(Type t, string method)
        {
            return GetOrAddMethodInfoToCache(t).FirstOrDefault(m => m.Name == method);
        }

        public static MethodInfo MakeGenericMethod(MethodInfo method, params Type[] typeArguments)
        {
            return _reflectionGenericMethodCache.GetOrAdd(typeArguments, method.MakeGenericMethod);
        }

        public static ParameterInfo[] FindMethodParameters(MethodInfo method)
        {
            return _reflectionMethodParameterCache.GetOrAdd(method, key => key.GetParameters());
        }

        public static ConstructorInfo FindConstructor(Type t, Type[] argTypes)
        {
            if (argTypes == null)
                argTypes = new Type[0];
            var ctors = _reflectionConstructorCache.GetOrAdd(t, key => new ConcurrentDictionary<Type[], ConstructorInfo>());
            return ctors.GetOrAdd(argTypes, key => t.GetConstructor(argTypes));
        }


        // Look into replacing with: 
        // http://weblogs.asp.net/gunnarpeipman/archive/2010/02/03/performance-using-dynamic-code-to-copy-property-values-of-two-objects.aspx  - JHE
        public static void CopyProperties<T>(T source, T destination)
        {
            CopyProperties(source, destination, null, null);
        }
        public static void CopyProperties<T>(T source, T destination, List<string> includedProperties, List<string> excludedProperties)
        {
            if (excludedProperties == null)
                excludedProperties = new List<string>();

            var props = Reflection.FindClassProperties(typeof(T));
            foreach (PropertyInfo pi in props)
            {
                if (includedProperties.IsNullOrEmpty())
                {
                    if (pi.CanRead && pi.CanWrite && !excludedProperties.ContainsIgnoreCase(pi.Name))
                        pi.SetValue(destination, pi.GetValue(source, null), null);
                }
                else
                {
                    if (pi.CanRead && pi.CanWrite && !excludedProperties.ContainsIgnoreCase(pi.Name) && includedProperties.ContainsIgnoreCase(pi.Name))
                        pi.SetValue(destination, pi.GetValue(source, null), null);
                }
            }
        }

        /// <summary>
        /// Method to copy value properties from 1 object to another using reflection and dynamic code.
        /// This is about 10X faster than the reflection only method when more that 1 call for specified
        /// same types is called. - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TU"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyPropertiesDynamic<T, TU>(T source, TU target)
        {
            NetSteps.Common.Reflection.CopyProperties.CopyWithDom(source, target);
        }



        public static bool AreEqualPropertyComparison<T>(T source, T destination)
        {
            return AreEqualPropertyComparison(source, destination, null, null);
        }
        public static bool AreEqualPropertyComparison<T>(T source, T destination, List<string> includedProperties, List<string> excludedProperties)
        {
            if (excludedProperties == null)
                excludedProperties = new List<string>();

            var props = Reflection.FindClassProperties(typeof(T));
            foreach (PropertyInfo pi in props)
            {
                if (includedProperties.IsNullOrEmpty())
                {
                    if (pi.CanRead && !excludedProperties.ContainsIgnoreCase(pi.Name))
                        if (pi.GetValue(source, null) != pi.GetValue(destination, null))
                            return false;
                }
                else
                {
                    if (pi.CanRead && !excludedProperties.ContainsIgnoreCase(pi.Name) && includedProperties.ContainsIgnoreCase(pi.Name))
                        if (pi.GetValue(source, null) != pi.GetValue(destination, null))
                            return false;
                }
            }

            return true;
        }


        public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                var props = type.GetPropertiesCached().ToList();
                foreach (System.Reflection.PropertyInfo pi in props)
                {
                    if (!ignoreList.Contains(pi.Name) && pi.CanRead)
                    {
                        object selfValue = pi.GetValue(self, null);
                        object toValue = pi.GetValue(to, null);

                        if (pi.PropertyType == typeof(DateTime))
                        {
                            DateTime dt1 = (DateTime)selfValue;
                            DateTime dt2 = (DateTime)toValue;

                            if (!dt1.IsEqualUpToSecond(dt2))
                                return false;
                        }
                        else if (pi.PropertyType == typeof(DateTime?))
                        {
                            DateTime? dt1 = (DateTime?)selfValue;
                            DateTime? dt2 = (DateTime?)toValue;

                            if (!dt1.IsEqualUpToSecond(dt2))
                                return false;
                        }
                        else if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return self == to;
        }


        public static List<PropertyComparisonValue> GetPublicInstancePropertiesWithDifferentValues<T>(T self, T to, params string[] ignore) where T : class
        {
            List<PropertyComparisonValue> results = new List<PropertyComparisonValue>();

            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                var props = type.GetPropertiesCached().ToList();
                foreach (System.Reflection.PropertyInfo pi in props)
                {
                    if (!ignoreList.Contains(pi.Name) && pi.CanRead)
                    {
                        object selfValue = pi.GetValue(self, null);
                        object toValue = pi.GetValue(to, null);

                        if (pi.PropertyType == typeof(DateTime))
                        {
                            DateTime dt1 = (DateTime)selfValue;
                            DateTime dt2 = (DateTime)toValue;

                            if (!dt1.IsEqualUpToSecond(dt2))
                                results.Add(new PropertyComparisonValue() { PropertyName = pi.Name, SourceValue = dt1.ToString(), DestinationValue = dt2.ToString() });
                        }
                        else if (pi.PropertyType == typeof(DateTime?))
                        {
                            DateTime? dt1 = (DateTime?)selfValue;
                            DateTime? dt2 = (DateTime?)toValue;

                            if (!dt1.IsEqualUpToSecond(dt2))
                                results.Add(new PropertyComparisonValue() { PropertyName = pi.Name, SourceValue = dt1.ToString(), DestinationValue = dt2.ToString() });
                        }
                        else if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            results.Add(new PropertyComparisonValue() { PropertyName = pi.Name, SourceValue = selfValue.ToString(), DestinationValue = toValue.ToString() });
                        }
                    }
                }
            }

            return results;
        }

        public class PropertyComparisonValue
        {
            public string PropertyName { get; set; }
            public string SourceValue { get; set; }
            public string DestinationValue { get; set; }
        }

        public static List<Type> SystemCoreTypes = new List<Type> { typeof(string), typeof(DateTime), typeof(DateTime?), typeof(bool), typeof(bool?), typeof(int), typeof(int?), typeof(short), typeof(short?), typeof(decimal), typeof(decimal?), typeof(double), typeof(double?) };


        public static IList CreateGenericListofType(Type type)
        {
            Type listT = typeof(List<>).MakeGenericType(new[] { type });
            //IList list = Activator.CreateInstance(listT) as IList;
            IList list = listT.NewFast() as IList;

            return list;
        }


        public static string GetPathToBin()
        {
            string path = Assembly.GetExecutingAssembly().CodeBase;
            path = path.Substring(0, path.LastIndexOf('/') + 1);
            if (path.StartsWith("file:///"))
                path = path.Replace("file:///", string.Empty);
            path = path.Substring(0, path.LastIndexOf('/') + 1);

            return path;
        }

        public static IDictionary<string, PropertyInfo> GetProperties(Type t, string field)
        {
            return _reflectionPropertyCache.GetOrAdd(t, key => new ConcurrentDictionary<string, PropertyInfo>(key.GetReadablePropertiesFromHierarchy(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name, p => p)));
        }

        public static PropertyInfo GetProperty(Type t, string field)
        {
            return GetProperties(t, field).Values.FirstOrDefault(p => p.Name == field);
        }

        //public static List<Assembly> GetAllAssebmliedInBin()
        //{
        //    try
        //    {
        //        List<Assembly> allAssemblies = new List<Assembly>();
        //        string path = GetPathToBin();
        //        foreach (string dll in Directory.GetFiles(path, "*.dll"))
        //            allAssemblies.Add(Assembly.LoadFile(dll));

        //        return allAssemblies;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
