using System;
using NetSteps.Silverlight.Exceptions;
using NetSteps.Silverlight.Threading;

namespace NetSteps.Silverlight.Base
{
    /// <summary>
    /// Author: John Egbert
    /// For client side caching of data that is cached in memory and Isolated storage.
    /// Created: 12/23/2009
    /// </summary>
    public abstract class EntityDictionaryCache<TKey, TValue> : ThreadSafeDictionary<TKey, TValue> where TValue : class
    {
        // TODO: Add optional Expiration date for the validity of the Isolated Storage Cache to reduce reloads
        //  and adjust/add timer to fire 1 update request from LIVE when the collection expires when reusing the 
        //  isolated storage version. - JHE
        // TODO: Store this dictionary of information in Isolated Storage and reload before returning data to track 
        //  when data should be reloaded/updated from live. - JHE
        //protected CacheDictionaryBase<TKey, DateTime> _updatedItems = new CacheDictionaryBase<TKey, DateTime>();
        //private TimeSpan _isolatedStorageTimeToLive = TimeSpan.FromMinutes(0);


        protected bool _updateInstanceOnAccess = false;
        public bool UpdateInstanceOnAccess
        {
            get
            {
                return _updateInstanceOnAccess;
            }
            set
            {
                _updateInstanceOnAccess = value;
            }
        }

        protected bool _useStaticCache = false;
        public bool UseStaticCache
        {
            get
            {
                return _useStaticCache;
            }
            set
            {
                _useStaticCache = value;
            }
        }

        protected bool _useIsolatedStorageCache = true;
        public bool UseIsolatedStorageCache
        {
            get
            {
                return _useIsolatedStorageCache;
            }
            set
            {
                _useIsolatedStorageCache = value;
            }
        }

        public EntityDictionaryCache()
        {
        }

        public virtual void GetInstance(TKey key, Action<TValue> callback)
        {
            bool useLocalVariable = false;
            if (UseStaticCache && List.ContainsKey(key))
                useLocalVariable = true;

            if (!useLocalVariable)
            {
                string fileName = GetFileName(key);
                if (UseIsolatedStorageCache && IsolatedStorage.ContainsFile(fileName))
                {
                    TValue cachedInstance = IsolatedStorage.LoadData<TValue>(fileName);
                    if (callback != null)
                        callback(cachedInstance);

                    if (UseStaticCache)
                        List[key] = cachedInstance;

                    if (UpdateInstanceOnAccess)
                    {
                        BackgroundAction.DoWork(() =>
                        {
                            UpdateInstanceFromLiveSource(key);  // Update data from LIVE - JHE
                        });
                    }
                }
                else
                    LoadInstance(key, callback);
            }
            else if (callback != null)
            {
                callback(List[key]);
            }
        }

        protected virtual bool IsInstanceCacheableViaIsolatedStorage(TValue item)
        {
            return true;
        }

        protected virtual string GetFileName(TKey key)
        {
            return string.Format("{0}-{1}-cache.dat", typeof(TValue).Name, key);
        }

        private void LoadInstance(TKey key, Action<TValue> callback)
        {
            LoadInstanceFromLive(key, (item) =>
            {
                lock (_lock)
                {
                    if (UseStaticCache)
                        List[key] = item;

                    if (UseIsolatedStorageCache && IsInstanceCacheableViaIsolatedStorage(item))
                        SaveInstanceToIsolatedStorage(key, item);
                }
                if (callback != null)
                    callback(item);
            });
        }

        protected abstract void LoadInstanceFromLive(TKey key, Action<TValue> callback);

        protected virtual void UpdateInstanceFromLiveSource(TKey key)
        {

        }

        protected virtual void RemoveFromCache(TKey key)
        {
            try
            {
                if (UseStaticCache && List.ContainsKey(key))
                    List.Remove(key);

                string fileName = GetFileName(key);
                if (IsolatedStorage.ContainsFile(fileName))
                    IsolatedStorage.DeleteFile(fileName);
            }
            catch (Exception ex)
            {
                AppFactory.ExceptionManager.HandleError(new GeneralException(ex));
            }
        }

        protected virtual void UpdateCache(TKey key, TValue item)
        {
            try
            {
                RemoveFromCache(key);

                if (UseStaticCache)
                    List[key] = item;

                if (UseIsolatedStorageCache && IsInstanceCacheableViaIsolatedStorage(item))
                    SaveInstanceToIsolatedStorage(key, item);
            }
            catch (Exception ex)
            {
                AppFactory.ExceptionManager.HandleError(new GeneralException(ex));
            }
        }

        protected virtual void SaveInstanceToIsolatedStorage(TKey key, TValue item)
        {
            if (UseIsolatedStorageCache)
            {
                // Do this in another thread; not UI thread (for performance) - JHE

                BackgroundAction.DoWork(() =>
                {
                    try
                    {
                        string fileName = GetFileName(key);
                        IsolatedStorage.SaveData<TValue>(item, fileName);
                    }
                    catch (Exception ex)
                    {
                        AppFactory.ExceptionManager.HandleError(new GeneralException(ex));
                    }
                });
            }
        }

    }
}
