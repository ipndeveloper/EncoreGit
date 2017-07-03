using System;
using System.Collections.Generic;
using System.ComponentModel;
using NetSteps.Common.Serialization;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A List class that inherits from the generic List class and implements 
    /// events for item addition and removals.
    /// Created: 01-20-2009
    /// </summary>
    [Serializable]
    public class BusinessListBase<T> : List<T>, ICloneable
    {
        #region Events
        /// <summary>
        /// Triggered whenever an item is added.
        /// </summary>
        public event EventHandler ItemAdded;

        /// <summary>
        /// Triggered whenever an item is removed.
        /// </summary>
        public event EventHandler ItemRemoved;
        #endregion

        #region Constructor
        public BusinessListBase()
        {
            // Add event handlers
            ItemAdded += OnItemAdded;
            ItemRemoved += OnItemRemoved;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles when an item is added to the collection.
        /// </summary>
        /// <param name="sender">The item added.</param>
        /// <param name="e">Event Arguments.</param>
        protected virtual void OnItemAdded(object sender, EventArgs e) { }

        /// <summary>
        /// Handles when an item is removed from the collection.
        /// </summary>
        /// <param name="sender">The item removed.</param>
        /// <param name="e">Event Arguments.</param>
        protected virtual void OnItemRemoved(object sender, EventArgs e) { }
        #endregion

        #region Adding
        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public new void Add(T item)
        {
            base.Add(item);

            if (ItemAdded != null)
                ItemAdded.Invoke(item, EventArgs.Empty);
        }

        /// <summary>
        /// Adds a range of items to the collection.
        /// </summary>
        /// <param name="items">Items to be added.</param>
        public new void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }
        #endregion

        #region Removing
        /// <summary>
        /// Removes a single item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was removed.</returns>
        public new bool Remove(T item)
        {
            bool _returned = base.Remove(item);

            if (_returned && ItemRemoved != null)
                ItemRemoved.Invoke(item, EventArgs.Empty);

            return _returned;
        }

        /// <summary>
        /// Removes all items that are matched.
        /// </summary>
        /// <param name="match">The predicate that provides matching.</param>
        /// <returns>The number of items removed.</returns>
        public new int RemoveAll(Predicate<T> match)
        {
            // If the predicate isn't valid, throw an exception.
            if (match == null)
                throw new ArgumentNullException("match", "Predicate was null.");

            int _count = 0;
            for (int i = 0; i < Count; i++)
            {
                T item = this[i];

                if (match(item))
                {
                    if (Remove(item)) _count++;
                }
            }

            return _count;
        }

        /// <summary>
        /// Remove an item at a specified index.
        /// </summary>
        /// <param name="index">The index at which to remove an item.</param>
        /// <returns>True if the item was removed.</returns>
        public new bool RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();

            return Remove(this[index]);
        }

        /// <summary>
        /// Removes a range of items from the collection.
        /// </summary>
        /// <param name="index">The start index for removing.</param>
        /// <param name="count">The number of items to remove.</param>
        public new void RemoveRange(int index, int count)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();

            if (count <= 0 || (index + count - 1) >= Count)
                throw new IndexOutOfRangeException();

            // Start from the back of the range to avoid
            // removal errors.
            for (int i = count + index - 1; i >= index; i--)
                Remove(this[i]);
        }

        /// <summary>
        /// Clears the collection of all items.
        /// </summary>
        public new void Clear()
        {
            if (Count <= 0)
                return;

            RemoveRange(0, Count);
        }
        #endregion

        #region ICloneable
        /// <summary>
        ///  To clone and return an object of same type.
        /// </summary>
        public T Clone()
        {
            return (T)GetClone();
        }

        Object ICloneable.Clone()
        {
            return this.GetClone();
        }

        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <returns>
        /// A new object containing the exact data of the original object. - JHE
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Object GetClone()
        {
            return ObjectCloner.Clone(this);
        }
        #endregion
    }
}
