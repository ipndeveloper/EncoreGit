using System;
using System.Collections.Generic;
using System.Configuration;

namespace NetSteps.Configuration
{
	public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T>
		where T : ConfigurationElement, new()
	{
		#region Properties

		protected Func<T, object> Key { get; private set; }

		public new string AddElementName
		{
			get { return base.AddElementName; }
			set { base.AddElementName = value; }
		}

		public new string ClearElementName
		{
			get { return base.ClearElementName; }
			set { base.AddElementName = value; }
		}

		public new string RemoveElementName { get { return base.RemoveElementName; } }

		public new int Count { get { return base.Count; } }

		public T this[int index]
		{
			get { return (T)BaseGet(index); }
			set
			{
				if(BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		new public T this[string hashCode]
		{
			get { return (T)BaseGet(hashCode); }
		}

		#endregion

		#region Constructors

		public GenericConfigurationElementCollection()
		{ }

		public GenericConfigurationElementCollection(Func<T, object> key)
		{
			if(key == null) throw new ArgumentNullException("key");
			Key = key;
		}

		#endregion

		#region Methods

		public int IndexOf(T item) { return BaseIndexOf(item); }

		public void Add(T item) { BaseAdd(item); }

		public void Remove(T item)
		{
			if(BaseIndexOf(item) >= 0) BaseRemove(item.GetHashCode().ToString());
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string hashCode)
		{
			BaseRemove(hashCode);
		}

		public void Clear()
		{
			BaseClear();
		}

		#region ConfigurationElementCollection Overrides

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new T();
		}

		protected override void BaseAdd(ConfigurationElement element)
		{
			BaseAdd(element, false);
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			object result = element.GetHashCode().ToString();
			if(Key != null && element is T) result = Key((T)element);
			return result;
		}

		public new IEnumerator<T> GetEnumerator()
		{
			for(int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#endregion
	}
}
