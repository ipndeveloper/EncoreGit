using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Collections
{
	public class ConcreteList<TAbstract, TConcrete> : IList<TConcrete>
		where TConcrete : TAbstract
	{
		#region Properties

		protected IList<TAbstract> Original
		{
			get;
			private set;
		}

		#endregion

		#region Construction

		public ConcreteList(IList<TAbstract> original)
		{
			Original = original;
		}

		#endregion

		#region Methods

		protected TAbstract ToAbstract(TConcrete value)
		{
			return (TAbstract)value;
		}

		protected TConcrete ToImplementation(TAbstract value)
		{
			return (TConcrete)value;
		}

		#endregion

		#region IList<TAbstraction> Members

		public int IndexOf(TConcrete item)
		{
			return Original.IndexOf(ToAbstract(item));
		}

		public void Insert(int index, TConcrete item)
		{
			Original.Insert(index, ToAbstract(item));
		}

		public void RemoveAt(int index)
		{
			Original.RemoveAt(index);
		}

		public TConcrete this[int index]
		{
			get
			{
				return ToImplementation(Original[index]);
			}
			set
			{
				Original[index] = ToAbstract(value);
			}
		}

		#endregion

		#region ICollection<TAbstraction> Members

		public void Add(TConcrete item)
		{
			Original.Add(ToAbstract(item));
		}

		public void Clear()
		{
			Original.Clear();
		}

		public bool Contains(TConcrete item)
		{
			return Original.Contains(ToAbstract(item));
		}

		public void CopyTo(TConcrete[] array, int arrayIndex)
		{
			Original.Cast<TConcrete>().ToList().CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return Original.Count; }
		}

		public bool IsReadOnly
		{
			get { return Original.IsReadOnly; }
		}

		public bool Remove(TConcrete item)
		{
			return Original.Remove(ToAbstract(item));
		}

		#endregion

		#region IEnumerable<TAbstraction> Members

		public IEnumerator<TConcrete> GetEnumerator()
		{
			return Original.Cast<TConcrete>().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Original.GetEnumerator();
		}

		#endregion
	}

}