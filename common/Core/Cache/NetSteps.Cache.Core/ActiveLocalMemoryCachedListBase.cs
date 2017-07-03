using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

using NetSteps.Encore.Core.IoC;
using System.Configuration;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// An active local memory implementation that will actively refresh itself.
	/// This happens upon first access or subsequent access following expiration.
	/// </summary>
	/// <typeparam name="T">The type stored in the List</typeparam>
	public abstract class ActiveLocalMemoryCachedListBase<T> : ICachedList<T>
	{
		#region Fields

		private ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim();
		private IList<T> _cache;
        private UtilElastiCache _utilElastiCache = new UtilElastiCache();

		#endregion

		#region Properties

		private TimeSpan LockTimeout { get; set; }

		/// <summary>
		/// The List's name. 
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// The total number of refresshes performed by this list
		/// </summary>
		public long TotalRefreshes { get; private set; }

		/// <summary>
		/// Indicates if the list is currently refreshing.
		/// </summary>
		public bool IsRefreshing { get; private set; }

		/// <summary>
		/// The TimeSpan from this point in time that the List should refresh.
		/// </summary>
		public TimeSpan RefreshesIn
		{
			get { return RefreshesAfter - DateTime.Now; }
		}

		/// <summary>
		/// The DateTime after which the List should refresh.
		/// </summary>
		public DateTime RefreshesAfter { get { return LastRefreshedOn.Add(RefreshInterval); } }

		/// <summary>
		/// The DateTime the List last refreshed.
		/// </summary>
		public DateTime LastRefreshedOn { get; protected set; }

		/// <summary>
		/// The TimeSpan interval in which the List refreshes.
		/// </summary>
		public TimeSpan RefreshInterval { get; set; }

		/// <summary>
		/// Returns the item at the given index
		/// </summary>
		/// <param name="index">The zero based index of the item to return</param>
		/// <returns>the item at the given index</returns>
		public T this[int index]
		{
			get
			{
				T result = default(T);
				Exception ex = null;
				if (EnsureReadable())
				{
					if (_syncLock.TryEnterReadLock(LockTimeout))
					{
						try
						{
							if (_cache.Count > index)
							{
								result = _cache[index];
							}
						}
						catch (Exception e)
						{
							ex = e;
						}
						finally
						{
							_syncLock.ExitReadLock();
						}
					}
				}
				if (ex != null)
				{
					throw ex;
				}
				return result;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// The count of items in the List
		/// </summary>
		public int Count
		{
			get
			{
				int c = 0;
				if (EnsureReadable())
				{
					if (_syncLock.TryEnterReadLock(LockTimeout))
					{
						try
						{
							c = _cache.Count;
						}
						finally
						{
							_syncLock.ExitReadLock();
						}
					}
				}
				return c;
			}
		}

		/// <summary>
		/// Indicates if the List is read only.  Always True.
		/// </summary>
		public bool IsReadOnly { get { return true; } }


		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new ActiveLocalMemoryCachedListBase with an Empty Name
		/// </summary>
		public ActiveLocalMemoryCachedListBase()
			: this(String.Empty)
		{ }

		/// <summary>
		/// Constructs a new ActiveLocalMemoryCachedListBase
		/// </summary>
		/// <param name="name">The name for this ActiveLocalMemoryCachedListBase</param>
        public ActiveLocalMemoryCachedListBase(string name)
        {
            LastRefreshedOn = DateTime.MinValue;
            TotalRefreshes = 0;

            MruCacheOptions opt = Config.CacheConfigSection.Current.NamedOrDefaultOptions<T>(name);

            Name = name;
            RefreshInterval = opt.CacheItemLifespan;

            LockTimeout = RefreshInterval.TotalSeconds > 30 ? TimeSpan.FromSeconds(30) : RefreshInterval;

            bool manejaElasticache = Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
            if (manejaElasticache)
            {
                // Método para guardar en cache lo que viene como nuevo - LBB
                if (!_utilElastiCache.LeerCache(name, out _cache))
                {

                }
            }
        }

		#endregion

		#region Methods

		/// <summary>
		/// Delegates the collection of the List data
		/// </summary>
		/// <returns>The List contents for this instance</returns>
		protected abstract IList<T> PerformRefresh();

		/// <summary>
		/// Allows an inheritor to perform some further logic after the List has refreshed
		/// </summary>
		protected virtual void AfterListRefreshed() { }

        private bool EnsureReadable()
        {
            bool refreshed = false;
            if (RefreshesIn <= TimeSpan.Zero)
            {
                if (_syncLock.TryEnterUpgradeableReadLock(LockTimeout))
                {
                    try
                    {
                        if (RefreshesIn <= TimeSpan.Zero)
                        {
                            if (_syncLock.TryEnterWriteLock(LockTimeout))
                            {
                                try
                                {
                                    if (_cache == null
                                        || RefreshesIn <= TimeSpan.Zero)
                                    {
                                        IsRefreshing = true;
                                        _cache = PerformRefresh();

                                        bool manejaElasticache = Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
                                        if (manejaElasticache)
                                        {
                                            _utilElastiCache.GuardarCacheTime(Name, _cache.ToList(), RefreshInterval);
                                        }

                                        LastRefreshedOn = DateTime.Now;
                                        TotalRefreshes++;
                                        refreshed = true;
                                    }
                                }
                                finally
                                {
                                    IsRefreshing = false;
                                    _syncLock.ExitWriteLock();
                                }
                            }
                        }
                    }
                    finally
                    {
                        _syncLock.ExitUpgradeableReadLock();
                    }
                }
            }
            if (refreshed)
            {
                AfterListRefreshed();
            }
            bool readable = false;
            if (_syncLock.TryEnterReadLock(LockTimeout))
            {
                try
                {
                    readable = _cache != null;
                }
                finally
                {
                    _syncLock.ExitReadLock();
                }
            }
            return readable;
        }

		/// <summary>
		/// Forces an immediate refresh of the Lists contents
		/// </summary>
		/// <returns>True if the List was successfully refreshed otherwise False</returns>
        protected bool Refresh()
        {
            bool refreshed = false;
            if (_syncLock.TryEnterWriteLock(LockTimeout))
            {
                try
                {
                    IsRefreshing = true;
                    _cache = PerformRefresh();

                    bool manejaElasticache = Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
                    if (manejaElasticache)
                    {
                        _utilElastiCache.GuardarCacheTime(Name, _cache.ToList(), RefreshInterval);
                    }

                    TotalRefreshes++;
                    LastRefreshedOn = DateTime.Now;
                    refreshed = _cache != null;
                }
                finally
                {
                    IsRefreshing = false;
                    _syncLock.ExitWriteLock();
                }
            }
            if (refreshed)
            {
                AfterListRefreshed();
            }
            return refreshed;
        }

		/// <summary>
		/// Returns the index of the given item in the List.
		/// </summary>
		/// <param name="item">The item to locate within the List</param>
		/// <returns>The zero based index of the item if found, otherwise -1</returns>
		public int IndexOf(T item)
		{
			int i = -1;
			if (EnsureReadable())
			{
				if (_syncLock.TryEnterReadLock(LockTimeout))
				{
					try
					{
						i = _cache.IndexOf(item);
					}
					finally
					{
						_syncLock.ExitReadLock();
					}
				}
			}

			return i;
		}

		/// <summary>
		/// This method is invalid due to the readonly nature of the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item) { throw new InvalidOperationException(); }

		/// <summary>
		/// This method is invalid due to the readonly nature of the list.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) { throw new InvalidOperationException(); }

		/// <summary>
		/// This method is invalid due to the readonly nature of the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item) { throw new InvalidOperationException(); }

		/// <summary>
		/// Supported for the IList interface.  
		/// This will trigger an immediate refresh of the List.
		/// </summary>
		public void Clear()
		{
			Refresh();
		}

		/// <summary>
		/// Indicates if the list contains the given item.
		/// </summary>
		/// <param name="item">The item to locate within the List</param>
		/// <returns>True if the List contains the item, else False</returns>
		public bool Contains(T item)
		{
			bool r = false;
			if (EnsureReadable())
			{
				if (_syncLock.TryEnterReadLock(LockTimeout))
				{
					try
					{
						r = _cache.Contains(item);
					}
					finally
					{
						_syncLock.ExitReadLock();
					}
				}
			}
			return r;
		}

		/// <summary>
		/// Copies the contents of the list to the given Array.
		/// </summary>
		/// <param name="array">The Array to copy to</param>
		/// <param name="arrayIndex">The point in the array in which to copy the List to.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			T[] a = new T[] { };
			if (EnsureReadable())
			{
				if (_syncLock.TryEnterReadLock(LockTimeout))
				{
					try
					{
						a = _cache.ToArray();
					}
					finally
					{
						_syncLock.ExitReadLock();
					}
				}
			}
			a.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// This method is invalid due to the readonly nature of the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item) { throw new InvalidOperationException(); }

		/// <summary>
		/// Gets an Enumerator over the contents of the List
		/// </summary>
		/// <returns>an Enumerator over the contents of the List</returns>
		public IEnumerator<T> GetEnumerator()
		{
			List<T> e = new List<T>();
			if (EnsureReadable())
			{
				if (_syncLock.TryEnterReadLock(LockTimeout))
				{
					try
					{
						e = _cache.ToList();
					}
					finally
					{
						_syncLock.ExitReadLock();
					}
				}
			}
			return e.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
