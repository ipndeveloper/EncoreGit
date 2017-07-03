using System;
using System.Collections.Concurrent;

namespace NetSteps.Common.Base
{

    public class AsyncReloadDictionary<K, V> where V : class
    {
        private readonly ConcurrentDictionary<K, AsyncReloadObject<V>> dictionary = new ConcurrentDictionary<K, AsyncReloadObject<V>>();
        private readonly Func<K, V> reloadFunction;
        private readonly TimeSpan reloadInterval;

        public AsyncReloadDictionary(Func<K, V> reloadFunction, TimeSpan reloadInterval = default(TimeSpan))
        {
            this.reloadFunction = reloadFunction;
            this.reloadInterval = reloadInterval;
        }

        public AsyncReloadObject<V> this[K key]
        {
            get
            {
                return dictionary.GetOrAdd(key, GetAsyncReloadObject);
            }
        }

        /// <summary>
        /// Clears all loaded data.  Cheap now, expensive later (lazy). See <seealso cref="AsyncReload"/>
        /// </summary>
        public void ExpireCache()
        {
            dictionary.Clear();
        }

        /// <summary>
		/// Reloads all stored data. Expensive now, cheap later (not lazy). See <seealso cref="ExpireCache"/>
        /// </summary>
        public void AsyncReload()
        {
            foreach (var lazyLoadObject in dictionary)
            {
                lazyLoadObject.Value.AsyncReload();
            }
        }

        public AsyncReloadObject<V> GetAsyncReloadObject(K key)
        {
            return new AsyncReloadObject<V>(() => reloadFunction(key), reloadInterval);
        }

        //private readonly ConcurrentDictionary<K, AsyncReloadObject<V>> _dictionary = new ConcurrentDictionary<K, AsyncReloadObject<V>>();
        //private readonly Func<K, V> _lazyLoadFunction;
        //private readonly TimeSpan _reloadInterval;

        //public AsyncReloadDictionary(Func<K, V> defaultLazyLoaderFunction, TimeSpan reloadInterval = default(TimeSpan))
        //{
        //    _lazyLoadFunction = defaultLazyLoaderFunction;
        //    _reloadInterval = reloadInterval;
        //}

        //private AsyncReloadObject<V> GetAsyncReloadObject(K key)
        //{
        //    return new AsyncReloadObject<V>(() => _lazyLoadFunction(key), _reloadInterval);
        //}

        //public AsyncReloadObject<V> this[K key]
        //{
        //    get
        //    {
        //        return _dictionary.GetOrAdd(key, GetAsyncReloadObject);
        //    }
        //}

        //public void ReloadAll()
        //{
        //    foreach (AsyncReloadObject<V> asyncObject in _dictionary.Values.ToList())
        //        asyncObject.ForceReload();
        //}


        //internal bool ContainsKey(K key)
        //{
        //    return _dictionary.ContainsKey(key);
        //}

        //internal void ReloadItem(K key)
        //{
        //    if (_dictionary.ContainsKey(key))
        //        this[key].ForceReload();
        //    else
        //        LoadItem(key);
        //}

        //internal void LoadItem(K key)
        //{
        //    if (this[key] == null)
        //        throw new Exception("Nothing is loaded!");
        //}

        //public DateTime? GetLastLoadedDate(K key)
        //{
        //    if (_dictionary.ContainsKey(key))
        //        return _dictionary[key].DateLastLoaded;
        //    else
        //        return null;
        //}
    }
}
