using System.Collections;
using System.Collections.Generic;

namespace NetSteps.Silverlight.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: This is basically a class wrapping a List<> with Thread safe 'locks' to allow this 
	/// 'instance' class intended to be used as a static variable to keep the data cached. 
	/// It also contains a few additional methods to wrap standard List<>
	/// methods and an Expire cache function.
	/// Created: 01-20-2009
	/// </summary>
	public class ThreadSafeList<T> : IList<T>
	{
		#region Members
		protected readonly object _lock = new object();
		protected List<T> _list;
		protected List<T> List
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
				return false;
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
			if (!this.Contains(item))
				List.Add(item);
		}

		public bool Remove(T item)
		{
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
			List.Clear();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			List.CopyTo(array, arrayIndex);
		}

		public void Insert(int index, T item)
		{
			List.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			List.RemoveAt(index);
		}

		public int IndexOf(T item)
		{
			return List.IndexOf(item);
		}

		public void ExpireCache()
		{
			lock (_lock)
			{
				_list = null;
			}
		}
		#endregion
	}
}