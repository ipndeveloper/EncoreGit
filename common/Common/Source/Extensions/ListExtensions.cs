using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: List Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class ListExtensions
    {
        public static void RemoveAll<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (list != null)
            {
                int i = 0;
                while (i < list.Count)
                {
                    if (condition(list[i]))
                        list.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

        public static void RemoveAll<T>(this IList<T> list)
        {
            if (list != null)
            {
                while (list.Count > 0)
                {
                    list.RemoveAt(0);
                }
            }
        }


        public static List<List<T>> SplitList<T>(this IEnumerable<T> source, int chunkSize)
        {
            List<List<T>> setsOfItems = new List<List<T>>();
            foreach (var list in source.Split(chunkSize).ToList())
            {
                List<T> listItems = new List<T>();
                listItems.AddRange(list);
                setsOfItems.Add(listItems);
            }
            return setsOfItems;
        }
    }
}
