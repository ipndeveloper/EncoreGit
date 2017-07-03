using System;
using System.Collections.Generic;
using NetSteps.Silverlight.Extensions;
using NetSteps.Silverlight.Threading;

namespace NetSteps.Silverlight.Base
{
    /// <summary>
    /// Author: John Egbert
    /// For client side caching of data that is cached in memory and Isolated storage.
    /// Created: 12/10/2009
    /// </summary>
    public abstract class EntityCollectionCache<T, C> : EntityCache<List<T>> where C : class, IList<T>, new()
    {
        // TODO: Add optional Expiration date for the validity of the Isolated Storage Cache to reduce reloads
        //  and adjust/add timer to fire 1 update request from LIVE when the collection expires when reusing the 
        //  isolated storage version. - JHE
        //private DateTime _isolatedStorageLastedUpdated = DateTime.MinValue;

        // Way to expire cache on a timeout out from RAM and then just reload from IS if needed again to reduce the memory footprint. - JHE
        private DispatcherTimerAction _expireCacheTimer = new DispatcherTimerAction();
        protected TimeSpan _unloadFromMemoryTimeout = TimeSpan.FromSeconds(0);
        public TimeSpan UnloadFromMemoryTimeout
        {
            get
            {
                return _unloadFromMemoryTimeout;
            }
            set
            {
                // Reset the timeout - JHE
                _expireCacheTimer.Stop();
                _unloadFromMemoryTimeout = value;
                _expireCacheTimer.Interval = _unloadFromMemoryTimeout;
                StartExpireCacheTimer();
            }
        }

        private DispatcherTimerAction _refreshCacheTimer = new DispatcherTimerAction();
        protected TimeSpan _refreshCacheTimeout = TimeSpan.FromSeconds(0);
        public TimeSpan RefreshCacheTimeout
        {
            get
            {
                return _refreshCacheTimeout;
            }
            set
            {
                // Reset the timeout - JHE
                _refreshCacheTimer.Stop();
                _refreshCacheTimeout = value;
                _refreshCacheTimer.Interval = _refreshCacheTimeout;
                StartRefreshCacheTimer();
            }
        }

        private C _cachedCollectionInstance = null;
        protected C CachedCollectionInstance
        {
            get
            {
                return _cachedCollectionInstance;
            }
            set
            {
                _cachedCollectionInstance = value;
                StartExpireCacheTimer();
            }
        }

        public bool IsInstanceLoaded
        {
            get
            {
                return _cachedCollectionInstance != null;
            }
        }

        public EntityCollectionCache()
        {
            FileName = string.Format("{0}-cache.dat", typeof(C).Name);

            _expireCacheTimer.Interval = UnloadFromMemoryTimeout;
            _expireCacheTimer.Action = () =>
            {
                _expireCacheTimer.Stop();
                ExpireCollectionInstanceCache();
            };
            StartExpireCacheTimer();

            _refreshCacheTimer.Interval = RefreshCacheTimeout;
            _refreshCacheTimer.Action = () =>
            {
                if (UpdateInstanceOnAccess)
                {
                    BackgroundAction.DoWork(() =>
                    {
                        UpdateInstanceFromLiveSource();  // Update data from LIVE - JHE
                    });
                }
            };
            StartRefreshCacheTimer();
        }

        public void GetInstance(Action<C> callback)
        {
            if (_cachedCollectionInstance == null)
            {
                if (IsolatedStorage.ContainsFile(FileName))
                {
                    lock (_lock)
                    {
                        // TODO: Add functionality to refresh the data periodically or every time the app loads (but in the background) - JHE
                        _cachedInstance = IsolatedStorage.LoadData<List<T>>(FileName);
                        if (_cachedInstance == null) // Error loading from Isolated Storage - JHE
                        {
                            LoadInstance(callback);
                            return;
                        }

                        _cachedCollectionInstance = new C();
                        foreach (T item in _cachedInstance)
                            _cachedCollectionInstance.Add(item);
                        StartExpireCacheTimer();
                    }

                    if (callback != null)
                        callback(_cachedCollectionInstance);

                    if (UpdateInstanceOnAccess)
                    {
                        BackgroundAction.DoWork(() =>
                        {
                            UpdateInstanceFromLiveSource();  // Update data from LIVE - JHE
                        });
                    }
                }
                else
                    LoadInstance(callback);
            }
            else if (callback != null)
                callback(_cachedCollectionInstance);
        }

        // Doesn't handle Errors loading - JHE
        public void LoadInstanceFromIsolatedStorage()
        {
            if (IsolatedStorage.ContainsFile(FileName))
            {
                lock (_lock)
                {
                    _cachedInstance = IsolatedStorage.LoadData<List<T>>(FileName);
                    if (_cachedInstance == null) // Error loading from Isolated Storage - JHE
                        return;

                    _cachedCollectionInstance = new C();
                    foreach (T item in _cachedInstance)
                        _cachedCollectionInstance.Add(item);
                }
            }
        }

        protected override void LoadInstance(Action<List<T>> callback)
        {
            throw new NotImplementedException();
        }

        protected abstract void LoadInstance(Action<C> callback);

        protected override void SaveInstanceToIsolatedStorage()
        {
            if (IsolatedStorage.StoreSize >= IsolatedStorage.MinimumIsolatedStorageSize)   // If they have enough space - JHE
            {
                // Do this in another thread; not UI thread (for performance) - JHE
                BackgroundAction.DoWork(() =>
                {
                    if (_cachedCollectionInstance != null && (_cachedInstance == null || _cachedInstance.Count != _cachedCollectionInstance.Count))
                    {
                        lock (_lock)
                        {
                            _cachedInstance = new List<T>();
                            foreach (T item in _cachedCollectionInstance)
                                _cachedInstance.Add(item);
                        }
                    }

                    if (_cachedInstance == null)
                    {
                        //IsolatedStorage.DeleteFile(FileName);
                    }
                    else
                        IsolatedStorage.SaveData<List<T>>(_cachedInstance, FileName);
                });
            }
        }

        protected void ExpireCollectionInstanceCache()
        {
            BackgroundAction.DoWork(() =>
            {
                lock (_lock)
                {
                    _cachedCollectionInstance = null;
                }
            });
        }

        private void StartExpireCacheTimer()
        {
            AppFactory.Dispatcher.BeginInvoke(() =>
            {
                if (!_expireCacheTimer.Interval.IsEmpty() && _cachedCollectionInstance != null)
                    _expireCacheTimer.Start();
            });
        }

        private void StartRefreshCacheTimer()
        {
            AppFactory.Dispatcher.BeginInvoke(() =>
            {
                if (!_refreshCacheTimer.Interval.IsEmpty())
                    _refreshCacheTimer.Start();
            });
        }
    }
}
