using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetSteps.Silverlight.Comparer;

namespace NetSteps.Silverlight.Extensions
{
    public static class IListExtensions
    {
        #region Validation Methods
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Conversion Methods
        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> value)
        {
            ObservableCollection<T> returnItems = new ObservableCollection<T>();
            foreach (T item in value)
            {
                returnItems.Add(item);
            }

            return returnItems;
        }
        #endregion

        public static List<T> Top<T>(this IList<T> value, int numberOfItems)
        {
            int count = 0;
            List<T> returnItems = new List<T>();
            while (count < numberOfItems)
            {
                foreach (T item in value)
                {
                    if (count >= numberOfItems)
                        break;

                    returnItems.Add(item);
                    count++;
                }
            }
            return returnItems;
        }

        /// <summary>
        /// Will get the next item in list comparing the current item by reference - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetNext<T>(this IList<T> value, T current) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (value[i] == current)
                {
                    if (i + 1 < value.Count)
                        return value[i + 1];
                    else
                        return null;
                }
            }
            return null;
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
        public static T GetNext<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (equalityComparer(value[i], current))
                {
                    if (i + 1 < value.Count)
                        return value[i + 1];
                    else
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Will get the next previous in list comparing the current item by reference - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T GetPrevous<T>(this IList<T> value, T current) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (value[i] == current)
                {
                    if (i - 1 >= 0)
                        return value[i - 1];
                    else
                        return null;
                }
            }
            return null;
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
        public static T GetPrevous<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (equalityComparer(value[i], current))
                {
                    if (i - 1 >= 0)
                        return value[i - 1];
                    else
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the index of an item comparing the current item by reference
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static int GetIndex<T>(this IList<T> value, T current) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (value[i] == current)
                    return i;
            }
            return 0;
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
        public static int GetIndex<T>(this IList<T> value, T current, Func<T, T, bool> equalityComparer) where T : class // So that null can be returned - JHE
        {
            for (int i = 0; i < value.Count; ++i)
            {
                if (equalityComparer(value[i], current))
                    return i;
            }
            return 0;
        }

        /// <summary>
        /// Method to do a one way sync of one collection to another. - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="syncToList"></param>
        /// <param name="equalityComparer"></param>
        /// <param name="updateItem"></param>
        /// <returns></returns>
        public static void SyncTo<T>(this IList<T> list, IList<T> syncToList, LambdaComparer<T> equalityComparer, Action<T, T> updateItem)
        {
            var existingItems = list.Distinct(equalityComparer);
            var itemsToAdd = syncToList.Except(existingItems, equalityComparer);
            var itemsToRemove = list.Except(existingItems, equalityComparer);



            // First update existing items - JHE
            // Lookups are reduced if Lambda Hash Method is available - JHE
            if (equalityComparer.IsLambdaHashMethodSet())
            {
                Dictionary<int, T> sourceItems = new Dictionary<int, T>();
                foreach (var item in syncToList)
                    sourceItems.Add(equalityComparer.GetHashCode(item), item);

                foreach (T item in existingItems)
                    if (sourceItems.ContainsKey(equalityComparer.GetHashCode(item)))
                        updateItem(item, sourceItems[equalityComparer.GetHashCode(item)]);

                //sourceItems
                //var items = existingItems.Join(syncToList, , , , equalityComparer);
                //    // TODO: Do a linq join here to reduce lookups and improve performance - JHE
                //    var items = from i in existingItems
                //                join u in syncToList on new { u., u.LastName } equals new { w.FirstName, w.LastName }
                //                //join u in syncToList on equalityComparer.Equals(i, u)
            }
            else
            {
                foreach (T item in existingItems)
                {
                    T updatedItem = syncToList.Single((singleItem) => equalityComparer.Equals(singleItem, item));
                    updateItem(item, updatedItem);
                }
            }

            foreach (var item in itemsToRemove)
                list.Remove(item);

            foreach (var item in itemsToAdd)
                list.Add(item);
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
        public static List<string> SortAndRemoveDuplicates(this IList<string> list)
        {
            var results = from l in list
                          orderby l
                          select l;

            return results.Distinct().ToList();
        }

        public static bool ContainsIgnoreCase(this IList<string> list, string stringValue)
        {
            foreach (string entry in list)
                if (entry.ToString().ToUpper() == stringValue.ToString().ToUpper())
                    return true;
            return false;
        }

        public static bool ContainsWordIgnoreCase(this IList<string> list, string stringValue)
        {
            List<string> keywords = stringValue.Replace(",", string.Empty).Replace(".", string.Empty).ToStringList(' ');

            foreach (string entry in list)
            {
                foreach (var keyword in keywords)
                {
                    if (entry.ToString().ToUpper() == keyword.ToString().ToUpper())
                        return true;
                }
            }
            return false;
        }

        public static bool ContainsPartialWordIgnoreCase(this IList<string> list, string stringValue)
        {
            List<string> keywords = stringValue.Replace(",", string.Empty).Replace(".", string.Empty).ToStringList(' ');

            foreach (string entry in list)
            {
                foreach (var keyword in keywords)
                {
                    if (keyword.ToString().ToUpper().Contains(entry.ToString().ToUpper()))
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}
