using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Common.Extensions
{
    public static class CollectionBaseExtensions
    {
        public static IEnumerable<T> Join<T>(this CollectionBase collection1, CollectionBase collection2)
        {
            return Join<T>(collection1, collection2, null);
        }

        public static IEnumerable<T> Join<T>(this CollectionBase collection1, CollectionBase collection2, IEqualityComparer<T> comparer)
        {
            List<T> all = new List<T>();
            foreach (T item in collection1)
            {
                all.Add(item);
            }
            foreach (T item in collection2)
            {
                if (comparer != null)
                {
                    if (!all.Contains<T>(item, comparer))
                    {
                        all.Add(item);
                    }
                }
                else
                {
                    if (!all.Contains(item))
                    {
                        all.Add(item);
                    }
                }
            }
            return all;
        }
    }
}
