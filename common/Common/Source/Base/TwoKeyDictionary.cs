using System;
using System.Collections.Concurrent;

namespace NetSteps.Common.Base
{
    public class TwoKeyDictionary<K1, K2, T> :
        ConcurrentDictionary<K1, ConcurrentDictionary<K2, T>>
    {
        
        public T GetOrAdd(K1 key1, K2 key2, Func<K1,K2,T> func)
        {
            ConcurrentDictionary<K2, T> dictionary = base.GetOrAdd(key1, x => new ConcurrentDictionary<K2, T>());
            return dictionary.GetOrAdd(key2, x => func.Invoke(key1, x));
        }


    }
}