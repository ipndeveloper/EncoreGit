using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Common.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helper class when dealing with an ordered list to enable iterating the list (previous, next) with a little more legible code.
	/// Created: 10-25-2010
	/// </summary>
	[Serializable]
	[System.Runtime.Serialization.CollectionDataContract(Name = "Ordered{0}List", ItemName = "Ordered{0}ListItem")]
	public class OrderedList<T> : List<T>
	{
		[System.Runtime.Serialization.DataMember(Name = "CurrentItem")]
		private T _currentItem = default(T);
		[System.Runtime.Serialization.DataMember(Name = "CurrentItemIndex")]
		private int _currentItemIndex = -1;

		[System.Runtime.Serialization.IgnoreDataMember]
		public T CurrentItem
		{
			get
			{
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				_currentItemIndex = this.IndexOf(_currentItem);
			}
		}
		[System.Runtime.Serialization.IgnoreDataMember]
		public int CurrentItemIndex
		{
			get
			{
				return _currentItemIndex;
			}
			set
			{
				_currentItemIndex = value;
				_currentItem = this[_currentItemIndex];
			}
		}

		public OrderedList(IEnumerable<T> items)
		{
			if (!items.Any())
				return;

			this.AddRange(items);
			CurrentItemIndex = 0;
		}

		public OrderedList()
			: base()
		{
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public T PreviousItem
		{
			get
			{
				if (HasPreviousItem)
					return this[PreviousItemIndex];
				else
					return default(T);
			}
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public T NextItem
		{
			get
			{
				if (HasNextItem)
					return this[NextItemIndex];
				else
					return default(T);
			}
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public int PreviousItemIndex
		{
			get
			{
				return CurrentItemIndex - 1;
			}
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public int NextItemIndex
		{
			get
			{
				return CurrentItemIndex + 1;
			}
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public bool HasPreviousItem
		{
			get
			{
				return (this.Any() && CurrentItemIndex > 0);
			}
		}

		[System.Runtime.Serialization.IgnoreDataMember]
		public bool HasNextItem
		{
			get
			{
				return (this.Any() && CurrentItemIndex + 1 < this.Count);
			}
		}
	}
}
