using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetSteps.Common.Comparer;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IList Extensions
    /// Created: 11-10-2009
    /// </summary>
    public static class IListExtensions
    {
        #region Validation Methods
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
        #endregion

        #region Conversion Methods
        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> value)
        {
            return new ObservableCollection<T>(value);
        }
        #endregion

        /// <summary>
        /// Will get the next item in list comparing the current item by reference - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetNext<T>(this IList<T> value, T current) //where T : class // So that null can be returned - JHE //default(T) is a better return for this - DES
        {
            int index = value.IndexOf(current);
            if (index > -1 && index < value.Count - 1)
                return value[index + 1];
            return default(T);
        }

        /// <summary>
        /// Will get the next item in list comparing the current item by using the provided equalityComparer - JHE
        /// Example:
        /// mailMessageModelCollection.GetNext(MailMessage, (MailMessageModel x, MailMessageModel y) => x.ExternalMessageID == y.ExternalMessageID);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static T GetNext<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer) //where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count - 1; ++i)
            {
                if (equalityComparer(value[i], current))
                {
                    return value[i + 1];
                }
            }
            return default(T);
        }

        /// <summary>
        /// Will get the next previous in list comparing the current item by reference - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetPrevous<T>(this IList<T> value, T current) //where T : class // So that null can be returned - JHE
        {
            int index = value.IndexOf(current);
            if (index > 0)
                return value[index - 1];
            return default(T);
        }

        /// <summary>
        /// Will get the previous item in list comparing the current item by using the provided equalityComparer - JHE
        /// Example:
        /// mailMessageModelCollection.GetPrevous(MailMessage, (MailMessageModel x, MailMessageModel y) => x.ExternalMessageID == y.ExternalMessageID);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static T GetPrevous<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer)// where T : class // So that null can be returned - JHE
        {
            for (int i = 1; i < value.Count; ++i)
            {
                if (equalityComparer(value[i], current))
                {
                    return value[i - 1];
                }
            }
            return default(T);
        }

        /// <summary>
        /// Returns the index of an item comparing the current item by reference
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static int GetIndex<T>(this IList<T> value, T current) //where T : class // So that null can be returned - JHE //default(T) is a better return for this - DES
        {
            return value.IndexOf(current);
        }

        /// <summary>
        /// Returns the index of an item comparing the current item by using the provided equalityComparer - JHE
        /// Example:
        /// mailMessageModelCollection.GetIndex(MailMessage, (MailMessageModel x, MailMessageModel y) => x.ExternalMessageID == y.ExternalMessageID);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static int GetIndex<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer) //where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (equalityComparer(value[i], current))
                    return i;
            }
            return -1;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
        {
            if (itemsToAdd != null)
            {
                foreach (T item in itemsToAdd)
                    list.Add(item);
            }
        }

        #region Sync List Methods

        /// <summary>
        /// Simple helper method that returns lists of items to Sync a collection with another. - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="syncToList"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static SyncLists<T> GetSyncToLists<T>(this IList<T> list, IEnumerable<T> syncToList, IEqualityComparer<T> equalityComparer)
        {
            SyncLists<T> returnList = new SyncLists<T>();
            returnList.ExistingItems = list.Distinct(equalityComparer).ToList();
            returnList.ItemsToAdd = syncToList.Except(returnList.ExistingItems, equalityComparer).ToList();
            //returnList.ItemsToRemove = list.Except(returnList.ExistingItems, equalityComparer).ToList();
            returnList.ItemsToRemove = returnList.ExistingItems.Except(syncToList, equalityComparer).ToList();
            return returnList;
        }

        /// <summary>
        /// Simple helper method that returns lists of items to Sync a collection with another.
        /// This Method varies from the one above in that it sync lists of IDs (SiteID, NavigationID, ect..) - JHE
        /// Example: 
        ///     List{int} accessibleSitesIds = new List{int}() { 1, 2, 3};
        ///     var syncLists = user.Sites.ToList(s => s.SiteID).GetSyncToLists(accessibleSitesIds);
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="syncToList"></param>
        /// <returns></returns>
        public static SyncLists<TValue> GetSyncToLists<TValue>(this IList<TValue> list, IEnumerable<TValue> syncToList)
        {
            SyncLists<TValue> returnList = new SyncLists<TValue>();
            returnList.ExistingItems = list.ToList();
            returnList.ItemsToAdd = syncToList.Except(returnList.ExistingItems).ToList();
            returnList.ItemsToRemove = list.Except(syncToList).ToList();
            return returnList;
        }
        public class SyncLists<T>
        {
            public IList<T> ExistingItems { get; set; }
            public IList<T> ItemsToAdd { get; set; }
            public IList<T> ItemsToRemove { get; set; }
        }

        public static void SyncTo<T, TProp>(this IList<T> list, IList<TProp> syncToList, Func<T, TProp> propertyToCompare, Func<TProp, T> addItem, Action<T> removeItem = null, bool removeItems = true)
        {
            if (syncToList == null)
                throw new ArgumentNullException("syncToList", "The list you are syncing to cannot be null.");

            var syncItems = list.ToList(propertyToCompare).GetSyncToLists(syncToList);

            if (removeItems)
            {
                foreach (var item in list.ToList())
                {
                    if (syncItems.ItemsToRemove.Contains(propertyToCompare(item)))
                    {
                        if (removeItem != null)
                            removeItem(item);
                        list.Remove(item);
                    }
                }
            }

            foreach (var item in syncItems.ItemsToAdd)
                list.Add(addItem(item));
        }



        /// <summary>
        /// Helper method to do a one way sync of one collection to another. - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="syncToList"></param>
        /// <param name="equalityComparer"></param>
        /// <param name="updateItem">First parameter is the old item, second is the updated item</param>
        /// <returns></returns>
        public static void SyncTo<T>(this IList<T> list, IEnumerable<T> syncToList, IEqualityComparer<T> equalityComparer, Action<T, T> updateItem, Action<IList<T>, T> removeItem = null)
        {
            if (syncToList == null)
                throw new ArgumentNullException("syncToList", "The list you are syncing to cannot be null.");

            var syncItems = GetSyncToLists(list, syncToList, equalityComparer);

            // First update existing items - JHE
            // Lookups are reduced if Lambda Hash Method is available - JHE
            if (updateItem != null)
            {
                var areAllHashValuesUnique = list.AreAllHashValuesUnique(equalityComparer);
                if (areAllHashValuesUnique)
                {
                    Dictionary<int, T> sourceItems = new Dictionary<int, T>();
                    foreach (var item in syncToList)
                        if (!sourceItems.ContainsKey(equalityComparer.GetHashCode(item)))
                            sourceItems.Add(equalityComparer.GetHashCode(item), item);

                    foreach (T item in syncItems.ExistingItems)
                        if (sourceItems.ContainsKey(equalityComparer.GetHashCode(item)))
                            updateItem(item, sourceItems[equalityComparer.GetHashCode(item)]);
                }
                else
                {
                    foreach (T item in syncItems.ExistingItems)
                    {
                        T updatedItem = syncToList.Single((singleItem) => equalityComparer.Equals(singleItem, item));
                        updateItem(item, updatedItem);
                    }
                }
            }



            foreach (var item in syncItems.ItemsToRemove)
            {
                if (removeItem != null)
                {
                    removeItem(list, item);
                }
                else
                {
                    list.Remove(item);
                }
            }


            foreach (var item in syncItems.ItemsToAdd)
                list.Add(item);
        }

        #endregion

        /// <summary>
        /// Method to remove and item(s) by a given function. - JHE
        /// Ex: account.Notes.RemoveWhere(n => n.NoteID == 1);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="equalityComparer"></param>
        public static IList<T> RemoveWhere<T>(this IList<T> value, Func<T, bool> equalityComparer)
        {
            foreach (var item in value.ToList())
            {
                if (equalityComparer(item))
                    value.Remove(item);
            }
            return value;
        }

        public static void Replace<T>(this IList<T> list, Func<T, bool> equalityComparer, T replacementItem)
        {
            list.RemoveWhere(equalityComparer);
            list.Add(replacementItem);
        }

        #region Random Methods
        public static T GetRandom<T>(this IList<T> value)
        {
            return value[Random.Next(0, value.Count)];
        }
        public static T GetRandom<T>(this IList<T> value, int minIndex)
        {
            return value[Random.Next(minIndex, value.Count)];
        }

        public static void RandomizeList<T>(this IList<T> value)
        {
            if (value.Count == 0)
                return;

            List<T> randomList = new List<T>();
            int randomIndex = 0;
            while (value.Count > 0)
            {
                randomIndex = Random.Next(0, value.Count);  // Choose a random object in the list
                randomList.Add(value[randomIndex]);         // Add it to the new, random List<T>
                value.RemoveAt(randomIndex);                // Remove to avoid duplicates
            }

            value.Clear();
            foreach (T element in randomList)
                value.Add(element);
        }
        #endregion

        #region Type Specific Extensions
        public static List<int> ToIntList(this IList<int?> list)
        {
            List<int> returnList = new List<int>();
            foreach (var item in list)
                returnList.Add(item.ToInt());
            return returnList;
        }

        public static IList<string> TrimWhitespace(this IList<string> list)
        {
            for (int i = 0; i < list.Count(); i++)
                list[i] = list[i].ToCleanString();

            return list;
        }
        #endregion
    }
}
