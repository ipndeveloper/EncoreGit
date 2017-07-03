using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Collections
{
	public class AbstractList<TImplementation,TAbstraction> : IList<TAbstraction>
		where TImplementation : TAbstraction 
	{
		#region Properties

		protected IList<TImplementation> Target
		{
			get;
			private set;
		}

		#endregion

		#region Construction

		public AbstractList(IList<TImplementation> target)
		{
			Target = target;
		}

		#endregion

		#region Methods

		protected TAbstraction ToAbstract(TImplementation value)
		{
			return (TAbstraction)value;
		}

		protected TImplementation ToImplementation(TAbstraction value)
		{
			return (TImplementation)value;
		}

		#endregion

		#region IList<TAbstraction> Members

		public int IndexOf(TAbstraction item)
		{
			return Target.IndexOf(ToImplementation(item));
		}

		public void Insert(int index, TAbstraction item)
		{
			Target.Insert(index, ToImplementation(item));
		}

		public void RemoveAt(int index)
		{
			Target.RemoveAt(index);
		}

		public TAbstraction this[int index]
		{
			get
			{
				return ToAbstract(Target[index]);
			}
			set
			{
				Target[index] = ToImplementation(value);
			}
		}

		#endregion

		#region ICollection<TAbstraction> Members

		public void Add(TAbstraction item)
		{
			Target.Add(ToImplementation(item));
		}

		public void Clear()
		{
			Target.Clear();
		}

		public bool Contains(TAbstraction item)
		{
			return Target.Contains(ToImplementation(item));
		}

		public void CopyTo(TAbstraction[] array, int arrayIndex)
		{
			Target.Cast<TAbstraction>().ToList().CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return Target.Count; }
		}

		public bool IsReadOnly
		{
			get { return Target.IsReadOnly; }
		}

		public bool Remove(TAbstraction item)
		{
			return Target.Remove(ToImplementation(item));
		}

		#endregion

		#region IEnumerable<TAbstraction> Members

		public IEnumerator<TAbstraction> GetEnumerator()
		{
			return Target.Cast<TAbstraction>().GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Target.GetEnumerator();
		}

		#endregion
	}
}
