using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IDictionary Extensions
    /// Created: 06-30-2010
    /// </summary>
    public static class IDictionaryExtensions
    {
        public static List<NameValue<TKey, TValue>> ToNameValueList<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            List<NameValue<TKey, TValue>> returnValue = new List<NameValue<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                returnValue.Add(new NameValue<TKey, TValue>(kvp.Key, kvp.Value));
            return returnValue;
        }
    }
}
