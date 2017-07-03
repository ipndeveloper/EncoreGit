using System;
using System.Collections;
using System.IO.IsolatedStorage;

namespace NetSteps.Silverlight
{
    public static class Variables
    {
        public static class IsolatedStorage
        {
            public static T Get<T>(string name)
            {
                return Variables.Get<T>(name, default(T), IsolatedStorageSettings.ApplicationSettings);
            }

            public static void Set<T>(string name, T value)
            {
                Variables.Set<T>(name, value, IsolatedStorageSettings.ApplicationSettings);
            }
        }

        private static T Get<T>(string name, IDictionary dictionary)
        {
            return Get<T>(name, default(T), dictionary);
        }

        private static T Get<T>(string name, T defaultValue, IDictionary dictionary)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (dictionary[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, dictionary[name].ToString(), true);
                        else
                            result = (T)Convert.ChangeType(dictionary[name].ToString(), objectType, null);

                        return result;
                    }
                }
                dictionary[name] = defaultValue;
                return defaultValue;
            }
            catch
            {
                dictionary[name] = defaultValue;
                return defaultValue;
            }
        }

        private static void Set<T>(string name, T variableValue, IDictionary dictionary)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    object obj = (T)result;
                    if (obj is Enum)
                        dictionary[name] = (T)Enum.Parse(objectType, variableValue.ToString(), true);
                    else
                        dictionary[name] = (T)variableValue;
                }
            }
            catch
            {
            }
        }
    }
}
