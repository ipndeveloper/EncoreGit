using System;
using System.Collections.Generic;

namespace NetSteps.Silverlight.Base
{
	/// <summary>
	/// This is basically a class wrapping a Dictionary<> with Thread safe 'locks' to allow this 
	/// 'instance' class intended to be used as a static variable to keep the data cached. 
	/// It also contains a few additional methods to wrap standard Dictionary<>
	/// methods and an Expire cache function. - JHE
	/// The ExpiringCacheDictionaryBase allows the addition of items that will expire at the Timespan specified. - JHE
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	public class ExpiringCachedDictionary<TKey, TValue>
	{
		#region Members
		protected readonly object _lock = new object();
		protected Dictionary<TKey, ExpiringItem<TValue>> _list;
		protected Dictionary<TKey, ExpiringItem<TValue>> List
		{
			get
			{
				if (_list == null)
				{
					lock (_lock)
					{
						if (_list == null)
							InitializeList();
					}
				}

				return _list;
			}
			set
			{
				lock (_lock)
				{
					_list = value;
				}
			}
		}

		protected TimeSpan _timeToLive = TimeSpan.FromMinutes(10);
		protected bool _isRollingExpiration = true;
		#endregion

		#region Properties
		public TimeSpan TimeToLive
		{
			get
			{
				return _timeToLive;
			}
			set
			{
				_timeToLive = value;
			}
		}

		public bool IsRollingExpiration
		{
			get
			{
				return _isRollingExpiration;
			}
			set
			{
				_isRollingExpiration = value;
			}
		}

		public virtual TValue this[TKey key]
		{
			get
			{
				if (this.List.ContainsKey(key))
				{
					if (this.List[key].ExpirationDate <= DateTime.Now)
					{
						this.List.Remove(key);
						return default(TValue);
					}
					else
						return this.List[key].Value;
				}
				else
					return default(TValue);
			}
			set
			{
				lock (_lock)
				{
					if (this.List.ContainsKey(key))
					{
						this.List[key].Value = value;
						if (IsRollingExpiration)
							this.List[key].ExpirationDate = DateTime.Now.Add(_timeToLive);
					}
					else
					{
						this.List[key] = new ExpiringCachedDictionary<TKey, TValue>.ExpiringItem<TValue>(_timeToLive, value);
					}
				}
			}
		}

		public int Count
		{
			get
			{
				return List.Count;
			}
		}

		public Dictionary<TKey, ExpiringItem<TValue>>.KeyCollection Keys
		{
			get
			{
				CleanOutExpiredItems();
				return List.Keys;
			}
		}

		#endregion

		#region Events
		public event EntityEventHandler<TValue> ItemExpired;
		protected virtual void OnItemExpired(object sender, EntityEventHandlerArgs<TValue> e)
		{
			if (ItemExpired != null)
				ItemExpired(this, e);
		}
		#endregion

		#region Methods
		public Dictionary<TKey, ExpiringItem<TValue>>.Enumerator GetEnumerator()
		{
			// Clean out expired items first? - JHE
			CleanOutExpiredItems();
			return List.GetEnumerator();
		}

		#region Helper Methods
		protected virtual void InitializeList()
		{
			lock (_lock)
			{
				_list = new Dictionary<TKey, ExpiringItem<TValue>>();
			}
		}
		#endregion

		public void Add(TKey key, TValue value)
		{
			lock (_lock)
			{
				if (!this.ContainsKey(key))
					List.Add(key, new ExpiringCachedDictionary<TKey, TValue>.ExpiringItem<TValue>(_timeToLive, value));
			}
		}

		public void Add(TKey key, TValue value, TimeSpan timeToLive)
		{
			lock (_lock)
			{
				if (!this.ContainsKey(key))
					List.Add(key, new ExpiringCachedDictionary<TKey, TValue>.ExpiringItem<TValue>(timeToLive, value));
			}
		}

		public void Remove(TKey key)
		{
			lock (_lock)
			{
				if (this.ContainsKey(key))
					ExpireItem(key);
			}
		}

		public TValue Value(TKey key)
		{
			CleanOutExpiredItemsByKey(key);

			foreach (var item in List)
				if (item.Key.Equals(key))
					return item.Value.Value;
			return default(TValue);
		}

		public bool ContainsKey(TKey key)
		{
			CleanOutExpiredItemsByKey(key);

			return List.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			CleanOutExpiredItems();
			foreach (var item in List.Values)
			{
				if (value.Equals(item.Value))
					return true;
			}
			return false;
		}

		public void ExpireCache()
		{
			lock (_lock)
			{
				_list = null;
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				List.Clear();
			}
		}

		public void CleanOutExpiredItemsByKey(TKey key)
		{
			if (List.ContainsKey(key))
				if (this.List[key].ExpirationDate <= DateTime.Now)
					ExpireItem(key);
		}

		public void CleanOutExpiredItems()
		{
			List<TKey> expiredItems = new List<TKey>();
			foreach (var key in List.Keys)
				if (this.List[key].ExpirationDate <= DateTime.Now)
					expiredItems.Add(key);

			foreach (var key in expiredItems)
				ExpireItem(key);
		}

		public void CleanOutExpiredItem(KeyValuePair<TKey, ExpiringItem<TValue>> item)
		{
			if (item.Value.ExpirationDate <= DateTime.Now)
				ExpireItem(item.Key);
		}

		protected void ExpireItem(TKey key)
		{
			OnItemExpired(this, new EntityEventHandlerArgs<TValue>(this.List[key].Value));
			List.Remove(key);
		}
		#endregion

		public class ExpiringItem<V>
		{
			public DateTime ExpirationDate { get; set; }
			public V Value { get; set; }

			public ExpiringItem(TimeSpan timeToLive, V value)
			{
				ExpirationDate = DateTime.Now.Add(timeToLive);
				Value = value;
			}

			public ExpiringItem(DateTime expirationDate, V value)
			{
				ExpirationDate = expirationDate;
				Value = value;
			}
		}
	}
}