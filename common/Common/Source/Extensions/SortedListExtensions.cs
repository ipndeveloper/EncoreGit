using System.Collections.Generic;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: Daniel Stafford
    /// Description: SortedList Extensions
    /// Created: 06-24-2009
    /// </summary>
    public static class SortedListExtensions
    {
        public static SortedList<int, T> ReIndex<T>(this SortedList<int, T> list)
        {
            for (int i = 1; i <= list.Keys.Count; i++)
            {
                if (!list.Keys.Contains(i))
                {
                    int j = ++i;
                    while (!list.Keys.Contains(j))
                    {
                        ++j;
                    }
                    T item = list[j];
                    list.RemoveAt(list.IndexOfKey(j));
                    list.Add(i, item);
                }
            }
            return list;
        }
    }
}
