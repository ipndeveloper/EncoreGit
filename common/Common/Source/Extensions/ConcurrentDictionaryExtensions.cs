using System.Collections.Concurrent;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ConcurrentDictionary Extensions
    /// Created: 06-30-2010
    /// </summary>
    public static class ConcurrentDictionaryExtensions
    {
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.AddOrUpdate(key, value, (dictKey, oldValue) => value);
        }
    }
}
