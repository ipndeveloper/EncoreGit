using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NetSteps.Extensibility.Core
{
    public static class ObjectExtensions
    {
        #region Recursive Execute
        public static void RecursiveExecuteForChildren<T>(this object targetObject, Action<T> actionToExecute)
        {
            List<object> alreadyProcessed = new List<object>();
            RecurseProperties<T>(targetObject, alreadyProcessed, actionToExecute);
        }

        private static void RecurseProperties<T>(object currentObject, List<object> alreadyProcessed, Action<T> action)
        {
            if (currentObject == null)
                return;
            alreadyProcessed.Add(currentObject);
            Type objType = currentObject.GetType();
            if (typeof(T).IsAssignableFrom(currentObject.GetType()))
                action((T)currentObject);
            IEnumerable<PropertyInfo> properties = objType.GetProperties();
            try
            {
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        Type valType = property.PropertyType;
                        if (valType.IsClass && !valType.IsPrimitive && !valType.IsValueType && valType != typeof(DateTime) && valType != typeof(string) && !valType.IsGenericParameter && !valType.Namespace.StartsWith("System.Reflection") && !valType.Namespace.StartsWith("System.Data.Metadata"))
                        {
                            try
                            {
                                ParameterInfo[] parameters = property.GetIndexParameters();
                                object value = property.GetValue(currentObject, parameters.Length == 0 ? null : new object[] { String.Empty });
                                if (value != null && !alreadyProcessed.Contains(value))
                                {
                                    if (typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                                    {
                                        alreadyProcessed.Add(value);
                                        foreach (object collectionItem in value as IEnumerable)
                                        {
                                            RecurseProperties(collectionItem, alreadyProcessed, action);
                                        }
                                    }
                                    else
                                        RecurseProperties(value, alreadyProcessed, action);
                                }
                            }
                            catch (TargetInvocationException)
                            {

                            }
                            catch (Exception ex)
                            {
                                var message = ex.Message;
                            }
                            finally
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                    }

                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        #endregion
    }
}
