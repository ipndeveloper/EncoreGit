using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Configuration
{
	/// <summary>
	/// Base class for configuration element collections.
	/// </summary>
	/// <typeparam name="TElement">Element type</typeparam>
	/// <typeparam name="TKey">Key type</typeparam>
	public abstract class AbstractConfigurationElementCollection<TElement, TKey> : ConfigurationElementCollection, IEnumerable<TElement>
		where TElement : ConfigurationElement, new()
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		protected AbstractConfigurationElementCollection()
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="addElmName">name used to add an element to the collection (default is 'add')</param>
		/// <param name="clearElmName">name used when clearing elements from the collection (default is 'clear')</param>
		/// <param name="removeElmName">name used to delete an element from the collection (default is 'remove')</param>
		protected AbstractConfigurationElementCollection(string addElmName
			, string clearElmName
			, string removeElmName)
		{
			base.AddElementName = addElmName;
			base.ClearElementName = clearElmName;
			base.RemoveElementName = removeElmName;
		}

		/// <summary>
		/// CollectionType
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		/// <summary>
		/// Number of elements.
		/// </summary>
		public new int Count
		{
			get { return base.Count; }
		}

		/// <summary>
		/// Accesses an element by index.
		/// </summary>
		/// <param name="index">element index</param>
		/// <returns>the element at <paramref name="index"/></returns>
		public TElement this[int index]
		{
			get { return (TElement)BaseGet(index); }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);

				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Accesses a element by key.
		/// </summary>
		/// <param name="key">an element's key</param>
		/// <returns>the element with the given key</returns>
		public TElement this[TKey key]
		{
			get { return (TElement)BaseGet(key); }
		}

		/// <summary>
		/// Adds an element.
		/// </summary>
		/// <param name="item"></param>
		public void Add(TElement item)
		{
			Contract.Requires<ArgumentNullException>(item != null);
			BaseAdd(item);
		}

		/// <summary>
		/// Clears the elements.
		/// </summary>
		public void Clear()
		{
			BaseClear();
		}

		/// <summary>
		/// Finds the index of an element.
		/// </summary>
		/// <param name="item">the element</param>
		/// <returns>the index of the element</returns>
		public int IndexOf(TElement item)
		{
			Contract.Requires<ArgumentNullException>(item != null);
			return BaseIndexOf(item);
		}

		/// <summary>
		/// Removes an element.
		/// </summary>
		/// <param name="item">the element</param>
		public void Remove(TElement item)
		{
			Contract.Requires<ArgumentNullException>(item != null);
			BaseRemove(GetElementKey(item));
		}

		/// <summary>
		/// Removes an element by key.
		/// </summary>
		/// <param name="key">the element's key</param>
		public void Remove(TKey key)
		{
			BaseRemove(key);
		}

		/// <summary>
		/// Removes an element at the given index.
		/// </summary>
		/// <param name="index">the element's index</param>
		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		/// <summary>
		/// Creates a new element of type TElement.
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new TElement();
		}

		/// <summary>
		/// Gets the element's key.
		/// </summary>
		/// <param name="element">the element</param>
		/// <returns>the element's key</returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			Contract.Assert(element != null);

			var result = PerformGetElementKey((TElement)element);
			Contract.Assume(result != null);
			return result;
		}

		/// <summary>
		/// Abstract method; gets the element's key.
		/// </summary>
		/// <param name="element">the element</param>
		/// <returns>the element's key</returns>
		protected abstract TKey PerformGetElementKey(TElement element);

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>an enumerator</returns>
		public new IEnumerator<TElement> GetEnumerator()
		{
			var e = base.GetEnumerator();
			while (e.MoveNext())
			{
				yield return (TElement)e.Current;
			}
		}
	}
}
