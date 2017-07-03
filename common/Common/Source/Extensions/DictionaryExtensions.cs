using System.Collections.Generic;

namespace NetSteps.Common.Extensions
{
	public static class DictionaryExtensions
	{
		public static Dictionary<TKey, TValue> Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			dictionary.Add(key, value);
			return dictionary;
		}

		public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> collection)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in collection)
			{
				dictionary.Add(kvp.Key, kvp.Value);
			}
			return dictionary;
		}
	}
}
