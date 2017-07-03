using System;
using System.Collections;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Common.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: This is basically a class wrapping a List{} with Thread safe 'locks' to allow this 
	///              'instance' class intended to be used as a static variable to keep the data cached. 
	///              It also contains a few additional methods to wrap standard List
	///              methods and an Expire cache function. - JHE
	/// Created: 12/14/2009
	/// </summary>
	[Serializable]
	public class CachedList<T> : IList<T>, IExpireCache
	{
		#region Members
		protected bool _isReadOnly = false;

		protected readonly object _lock = new object();
		protected List<T> _list;
		protected List<T> List
		{
			get
			{
				lock (this._lock)
				{
					if (this._list == null)
					{
					    InitializeList();
					}
					return this._list;
				}
			}
			set
			{
				lock (_lock)
				{
					if (_isReadOnly)
						throw new Exception("This list is read-only.");

					_list = value;
				}
			}
		}
		#endregion

		#region Properties
		public T this[int index]
		{
			get
			{
				return this.List[index];
			}
			set
			{
				lock (_lock)
				{
					if (_isReadOnly)
						throw new Exception("This list is read-only.");

					this.List[index] = value;
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

		public bool IsReadOnly
		{
			get
			{
				return _isReadOnly;
			}
		}

		public bool IsValueCreated
		{
			get
			{
				return !(_list == null);
			}
		}
		#endregion

		#region Methods

		#region Helper Methods
		protected virtual void InitializeList()
		{
			lock (_lock)
			{
				_list = new List<T>();
			}
		}
		#endregion

		IEnumerator IEnumerable.GetEnumerator()
		{
			return List.GetEnumerator();
		}
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return List.GetEnumerator();
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return List.GetEnumerator();
		}

		public void Add(T item)
		{
			if (_isReadOnly)
				throw new Exception("This list is read-only.");
			if (!this.Contains(item))
				List.Add(item);
		}

		public bool Remove(T item)
		{
			if (_isReadOnly)
				throw new Exception("This list is read-only.");
			if (this.Contains(item))
				return List.Remove(item);

			return false;
		}

		public bool Contains(T item)
		{
			return List.Contains(item);
		}

		public void Clear()
		{
			if (_isReadOnly)
				throw new Exception("This list is read-only.");
			List.Clear();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			List.CopyTo(array, arrayIndex);
		}

		public void Insert(int index, T item)
		{
			if (_isReadOnly)
				throw new Exception("This list is read-only.");
			List.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			if (_isReadOnly)
				throw new Exception("This list is read-only.");
			List.RemoveAt(index);
		}

		public int IndexOf(T item)
		{
			return List.IndexOf(item);
		}

		public virtual void ExpireCache()
		{
			lock (_lock)
			{
				_list = null;
			}
		}
		#endregion
	}
}