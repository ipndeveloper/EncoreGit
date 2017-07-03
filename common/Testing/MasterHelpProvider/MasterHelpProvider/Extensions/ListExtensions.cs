using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.DataFaker
{
   public static class ListExtensions
    {
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

        public static bool ContainsIgnoreCase(this List<string> list, string stringValue)
        {
            foreach (string entry in list)
                if (entry.ToString().ToUpper() == stringValue.ToString().ToUpper())
                    return true;
            return false;
        }

        public static List<T> Each<T>(this List<T> list, Action<T> operation)
        {
            if (list != null && list.Count > 0)
            {
                foreach (T item in list)
                {
                    operation(item);
                }
            }
            return list;
        }

        public static List<TResult> Each<T, TResult>(this List<T> list, Func<T, TResult> operation)
        {
            if (list != null)
            {
                if (typeof(TResult) == typeof(void))
                {
                    foreach (T item in list)
                    {
                        operation(item);
                    }
                    return null;
                }
                else
                {
                    List<TResult> results = new List<TResult>();
                    foreach (T item in list)
                    {
                        results.Add(operation(item));
                    }
                    return results;
                }
            }
            return null;
        }

        public static TReturn Each<T, TResult, TReturn>(this List<T> list, Func<T, TResult> operation) where TReturn : List<TResult>
        {
            if (list != null)
            {
                if (typeof(TResult) == typeof(void))
                {
                    foreach (T item in list)
                    {
                        operation(item);
                    }
                    return null;
                }
                else
                {
                    List<TResult> results = new List<TResult>();
                    foreach (T item in list)
                    {
                        results.Add(operation(item));
                    }
                    return (TReturn)results;
                }
            }
            return null;
        }
    }
}
