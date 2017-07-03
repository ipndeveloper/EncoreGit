using System.Windows;

namespace NetSteps.Silverlight.Extensions
{
    public static class ResourceDictionaryExtensions
    {
        public static void ReplaceStyle(this ResourceDictionary source, string key, ResourceDictionary resourceDictionary)
        {
            // Only replace the style if a style with the same key exists in the custom resource dictionary. - JHE
            if (resourceDictionary[key] != null)
            {
                source.Remove(key);
                source.Add(key, resourceDictionary[key]);
            }
        }

        public static void AddSafe(this ResourceDictionary source, string key, object value)
        {
            if (source.Contains(key))
                source.Remove(key);
            source.Add(key, value);
        }

        public static void RemoveSafe(this ResourceDictionary source, string key)
        {
            if (source.Contains(key))
                source.Remove(key);
        }

    }
}
