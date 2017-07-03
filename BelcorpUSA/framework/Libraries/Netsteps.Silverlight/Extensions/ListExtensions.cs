using System.Collections.Generic;

namespace NetSteps.Silverlight.Extensions
{
    public static class ListExtensions
    {
        public delegate bool Predicate<T>(T arg);
        public static void RemoveAll<T>(this IList<T> list, Predicate<T> condition)
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
    }
}
