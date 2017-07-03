using System.Collections.Generic;

namespace NetSteps.Silverlight.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: This is basically a class wrapping a Dictionary<> with Thread safe 'locks' to allow this 
	/// 'instance' class intended to be used as a static variable to keep the data cached. 
	/// It also contains a few additional methods to wrap standard Dictionary<>
	/// methods and an Expire cache function.
	/// Created: 01-20-2009
	/// </summary>
	public class ThreadSafeDictionary<TKey, TValue>
	{
		#region Members
		protected readonly object _lock = new object();
		protected Dictionary<TKey, TValue> _list;
		protected Dictionary<TKey, TValue> List
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
		#endregion

		#region Properties
		public virtual TValue this[TKey key]
		{
			get
			{
				if (this.List.ContainsKey(key))
					return this.List[key];
				else
					return default(TValue);
			}
			set
			{
				lock (_lock)
				{
					this.List[key] = value;
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

		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return List.Keys;
			}
		}

		#endregion

		#region Methods
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return List.GetEnumerator();
		}

		#region Helper Methods
		protected virtual void InitializeList()
		{
			lock (_lock)
			{
				_list = new Dictionary<TKey, TValue>();
			}
		}
		#endregion

		public virtual void Add(TKey key, TValue value)
		{
			lock (_lock)
			{
				if (!this.ContainsKey(key))
					List.Add(key, value);
			}
		}

		public virtual void Remove(TKey key)
		{
			lock (_lock)
			{
				if (this.ContainsKey(key))
					List.Remove(key);
			}
		}

		public TValue Value(TKey key)
		{
			foreach (KeyValuePair<TKey, TValue> item in List)
				if (item.Key.Equals(key))
					return item.Value;
			return default(TValue);
		}

		public bool ContainsKey(TKey key)
		{
			return List.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return List.ContainsValue(value);
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
		#endregion
	}
}