using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NetSteps.Common.Comparer;
using NetSteps.Common.Expressions;
using NetSteps.Common.Interfaces;
using System.Diagnostics.Contracts;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IEnumerable Extensions
    /// Created: 06-19-2009
    /// </summary>
    public static class IEnumerableExtensions
    {
        [Obsolete("This method does not always work correctly. See the notes in LambdaComparer.cs and use DistinctBy() instead. - Lundy")]
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> equalityComparer)
        {
            return source.Distinct(new LambdaComparer<T>(equalityComparer));
        }
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> equalityComparer, Func<T, int> lambdaHash)
        {
            return source.Distinct(new LambdaComparer<T>(equalityComparer, lambdaHash));
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.Distinct(new LambdaEqualityComparer<T, TKey>(keySelector));
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.Distinct(new LambdaEqualityComparer<T, TKey>(keySelector, comparer));
        }

        public static IEnumerable<T> UnionBy<T, TKey>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, TKey> keySelector)
        {
            return first.Union(second, new LambdaEqualityComparer<T, TKey>(keySelector));
        }
        public static IEnumerable<T> UnionBy<T, TKey>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return first.Union(second, new LambdaEqualityComparer<T, TKey>(keySelector, comparer));
        }

        public static IEnumerable<T> IntersectBy<T, TKey>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, TKey> keySelector)
        {
            return first.Intersect(second, new LambdaEqualityComparer<T, TKey>(keySelector));
        }
        public static IEnumerable<T> IntersectBy<T, TKey>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return first.Intersect(second, new LambdaEqualityComparer<T, TKey>(keySelector, comparer));
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> list, Action<T> operation)
        {
            if (list != null && list.Count() > 0)
            {
                foreach (T item in list)
                {
                    operation(item);
                }
            }
            return list;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> list, Action<T, int> operation)
        {
            if (list != null && list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    operation(list.ElementAt(i), i);
                }
            }
            return list;
        }

        public static IEnumerable<TResult> Each<T, TResult>(this IEnumerable<T> list, Func<T, TResult> operation)
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

        public static IEnumerable<TResult> Each<T, TResult>(this IEnumerable<T> list, Func<T, int, TResult> operation)
        {
            if (list != null)
            {
                List<TResult> results = new List<TResult>();
                for (int i = 0; i < list.Count(); i++)
                {
                    results.Add(operation(list.ElementAt(i), i));
                }
                return results;
            }
            return null;
        }

        public static TReturn Each<T, TResult, TReturn>(this IEnumerable<T> list, Func<T, TResult> operation) where TReturn : IEnumerable<TResult>
        {
            if (list != null)
            {
                if (typeof(TResult) == typeof(void))
                {
                    foreach (T item in list)
                    {
                        operation(item);
                    }
                    return default(TReturn);
                }
                else
                {
                    IEnumerable<TResult> results = new List<TResult>();
                    foreach (T item in list)
                    {
                        (results as List<TResult>).Add(operation(item));
                    }
                    return (TReturn)results;
                }
            }
            return default(TReturn);
        }

        public static TReturn Each<T, TResult, TReturn>(this IEnumerable<T> list, Func<T, int, TResult> operation) where TReturn : IEnumerable<TResult>
        {
            if (list != null)
            {
                if (typeof(TResult) == typeof(void))
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        operation(list.ElementAt(i), i);
                    }
                    return default(TReturn);
                }
                else
                {
                    IEnumerable<TResult> results = new List<TResult>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        (results as List<TResult>).Add(operation(list.ElementAt(i), i));
                    }
                    return (TReturn)results;
                }
            }
            return default(TReturn);
        }

        public static string ToString<T>(this IEnumerable<T> list, Func<T, string> str)
        {
            if (list != null && list.Count() > 0)
            {
                StringBuilder builder = new StringBuilder();
                list.Each(i => builder.Append(str(i)));
                return builder.ToString();
            }
            return string.Empty;
        }

        public static string ToString<T>(this IEnumerable<T> list, Func<T, int, string> op)
        {
            if (list != null && list.Count() > 0)
            {
                StringBuilder builder = new StringBuilder();
                list.Each((item, i) => builder.Append(op(item, i)));
                return builder.ToString();
            }
            return "";
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string property)
        {
            return list.Order(property, "OrderBy");
        }

        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> list, string property)
        {
            return list.Order(property, "OrderByDescending");
        }

        public static IOrderedEnumerable<T> ThenBy<T>(this IEnumerable<T> list, string property)
        {
            return list.Order(property, "ThenBy");
        }

        public static IOrderedEnumerable<T> ThenByDescending<T>(this IEnumerable<T> list, string property)
        {
            return list.Order(property, "ThenByDescending");
        }

        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> list, string property, string methodToInvoke)
        {
            string[] props = property.Split('.');
            Type type = list.GetTypeCheckSingle();
            Type propertyType = null;

            ParameterExpression p = Expression.Parameter(type, "x");

            Expression expr = p;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetPropertyCached(prop);
                if (pi == null)
                    throw new ArgumentException(string.Format("Property '{0}' does not exist on type '{1}'", property, type.Name), "property");
                expr = Expression.Property(expr, pi);
                propertyType = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(type, propertyType);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, p);

            MethodInfo orderMethod = typeof(Enumerable).GetMethodsCached().FirstOrDefault(m => m.Name == methodToInvoke && m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false));

            if (orderMethod != default(MethodInfo))
            {
                return orderMethod.MakeGenericMethod(type, propertyType).Invoke(null, new object[] { list, lambda.Compile() }) as IOrderedEnumerable<T>;
            }
            return default(IOrderedEnumerable<T>);
        }

        private static MethodInfo whereMethod;

        public static IEnumerable<T> Where<T>(this IEnumerable<T> list, string property, ComparisonType compType, object value)
        {
            if (whereMethod == null)
            {
                Func<T, bool> fakeKeySelector = element => default(bool);
                Expression<Func<IEnumerable<T>, IEnumerable<T>>> lamda = l => l.Where(fakeKeySelector);
                whereMethod = (lamda.Body as MethodCallExpression).Method;
            }

            var results = whereMethod.Invoke(null, new object[] { list, ExpressionHelper.MakeExpression<T, bool>(property, compType, value) }) as IEnumerable<T>;
            if (results != null)
                return results;
            return default(IEnumerable<T>);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null || selector == null || comparer == null)
                throw new Exception("source and selector and comparer, cannot be null");

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }


        public static IEnumerable<T> Complement<T>(this IEnumerable<T> list, IEnumerable<T> listToComplement, Func<T, T, bool> comparer)
        {
            return list.Where(i => listToComplement.FirstOrDefault(i2 => comparer(i, i2)).Equals(default(T)));
        }


        /// <summary>
        /// Returns a generic list of the property values specified in the property parameter. - JHE
        /// Example: var roleFunctionIds = r.Functions.ToList(f => f.FunctionID);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="list"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static List<TProp> ToList<T, TProp>(this IEnumerable<T> list, Func<T, TProp> property)
        {
            return list.Select(property).ToList();
        }

        /// <summary>
        /// A wrapper method for Except that takes a Func parameter instead of the IEqualityComparer - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, IEnumerable<T> second, Func<T, T, bool> lambdaComparer)
        {
            return list.Except(second, new LambdaComparer<T>(lambdaComparer));
        }

        public static string Join<T>(this IEnumerable<T> list, string separator)
        {
            return string.Join(separator, list);
        }

        public static IEnumerable<T> Flatten<T, TList>(this IEnumerable<TList> listOfLists) where TList : IEnumerable<T>
        {
            List<T> list = new List<T>();
            listOfLists.Each(l => list.AddRange(l));
            return list;
        }


        #region Type Specific Extensions
        public static List<string> ToPropertyNames(this IEnumerable<PropertyInfo> list)
        {
            return list.Select(p => p.Name).ToList();
        }

        public static List<IListValue> ToIListValueList(this IEnumerable<IListValue> list)
        {
            return list.ToList();
        }

        public static string ToCommaSeparatedString<T>(this IEnumerable<T> list)
        {
            return list.Join(", ");
        }

        public static bool ContainsIgnoreCase(this IEnumerable<string> items, string value)
        {
            Contract.Requires<ArgumentNullException>(items != null);
            return items.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Will return a incremented int of the property in the keySelector if the value is greater than 0. - JHE
        /// Example: region.ShippingRegionWarehouses.GetNextInt(sw => sw.SortIndex)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static int GetNextInt<TSource>(this IEnumerable<TSource> source, Func<TSource, int> keySelector)
        {
            int maxIndex = -1;
            if (source != null && source.Count() > 0)
                maxIndex = source.Max(keySelector);
            if (maxIndex > -1)
                return maxIndex + 1;
            else
                return maxIndex;
        }


        /// <summary>
        /// Loops through items and cleans up all the SortIndex properties on the items. - JHE
        /// </summary>
        /// <param name="source"></param>
        public static void ReIndex(this IEnumerable<ISortIndex> source)
        {
            source.ReIndex(sw => sw.SortIndex, (ISortIndex sw, int id) => sw.SortIndex = id);
        }

        /// <summary>
        /// Loops through items and cleans up all the SortIndex properties on the items. - JHE
        /// </summary>
        /// <param name="source"></param>
        public static void Move(this IEnumerable<ISortIndex> source, int sortIndex, Constants.SortDirection direction)
        {
            foreach (var items in source.OrderBySortIndex())
            {
                switch (direction)
                {
                    case Constants.SortDirection.Ascending:
                        if (items.SortIndex == sortIndex - 1)
                        {
                            items.SortIndex = items.SortIndex + 1;
                        }
                        else if (items.SortIndex == sortIndex)
                        {
                            items.SortIndex = items.SortIndex - 1;
                        }
                        break;
                    case Constants.SortDirection.Descending:
                        if (items.SortIndex == sortIndex + 1)
                        {
                            items.SortIndex = items.SortIndex - 1;
                        }
                        else if (items.SortIndex == sortIndex)
                        {
                            items.SortIndex = items.SortIndex + 1;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Loops through items and "re-indexes" the property passed into the paramaters. - JHE
        /// Example:
        ///     region.ShippingRegionWarehouses.ReIndex(sw => sw.SortIndex, (ShippingRegionWarehouse sw, int id) => sw.SortIndex = id);
        /// </summary>
        /// <param name="source"></param>
        public static void ReIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, int> keySelector, Action<TSource, int> setKeySelector)
        {
            int count = 1;
            foreach (TSource item in source.OrderBy(keySelector))
            {
                setKeySelector(item, count);
                count++;
            }
        }

        public static IOrderedEnumerable<ISortIndex> OrderBySortIndex(this IEnumerable<ISortIndex> source)
        {
            return source.OrderBy(sw => sw.SortIndex);
        }


        public static int GetNextIntNegative<TSource>(this IEnumerable<TSource> source, Func<TSource, int> keySelector)
        {
            int minIndex = 0;
            if (source != null && source.Count() > 0)
                minIndex = source.Min(keySelector);
            return minIndex - 1;
        }
        #endregion

        //This seems to screw up C#'s ability to determine between the strongly typed version and this, so I'm commenting it out for now - DES
        //public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable list, Func<dynamic, TKey> keySelector, Func<dynamic, TValue> valueSelector)
        //{
        //    Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        //    foreach (var item in list)
        //    {
        //        dict.Add(keySelector(item), valueSelector(item));
        //    }
        //    return dict;
        //}

        //public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<dynamic> list, Func<dynamic, TKey> keySelector, Func<dynamic, TValue> valueSelector)
        //{
        //    Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        //    foreach (var item in list)
        //    {
        //        dict.Add(keySelector(item), valueSelector(item));
        //    }
        //    return dict;
        //}


        public static IEnumerable<T> WithInCurrentMonth<T>(this IEnumerable<T> list, Func<T, DateTime> dateTimeSelector)
        {
            list = list.Where(s => dateTimeSelector(s) >= DateTime.Now.ApplicationNow().FirstDayOfMonth().Midnight() && dateTimeSelector(s) <= DateTime.Now.ApplicationNow().LastDayOfMonth().EndOfDay()).ToList();
            return list;
        }
        public static IEnumerable<T> WithInMonth<T>(this IEnumerable<T> list, Func<T, DateTime> dateTimeSelector, int month)
        {
            DateTime monthDate = new DateTime(DateTime.Now.ApplicationNow().Year, month, 1);
            list = list.Where(s => dateTimeSelector(s) >= monthDate.FirstDayOfMonth().Midnight() && dateTimeSelector(s) <= monthDate.LastDayOfMonth().EndOfDay()).ToList();
            return list;
        }
        public static IEnumerable<T> WithInDayRange<T>(this IEnumerable<T> list, Func<T, DateTime> dateTimeSelector, DateTime startDate, DateTime endDate)
        {
            list = list.Where(s => dateTimeSelector(s) >= startDate.Midnight() && dateTimeSelector(s) <= endDate.EndOfDay()).ToList();
            return list;
        }

        // Count method that will return 0 if the list is null, otherwise return the Count of item in the list. - JHE
        public static int CountSafe<T>(this IEnumerable<T> list)
        {
            if (list == null)
                return 0;
            else
                return list.Count();
        }



        /// <summary>Adds a single element to the end of an IEnumerable.</summary> 
        /// <typeparam name="T">Type of enumerable to return.</typeparam> 
        /// <returns>IEnumerable containing all the input elements, followed by the 
        /// specified additional element.</returns> 
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            return concatIterator(element, source, false);
        }

        /// <summary>Adds a single element to the start of an IEnumerable.</summary> 
        /// <typeparam name="T">Type of enumerable to return.</typeparam> 
        /// <returns>IEnumerable containing the specified additional element, followed by 
        /// all the input elements.</returns> 
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> tail, T head)
        {
            if (tail == null)
                throw new ArgumentNullException("tail");
            return concatIterator(head, tail, true);
        }

        private static IEnumerable<T> concatIterator<T>(T extraElement,
            IEnumerable<T> source, bool insertAtStart)
        {
            if (insertAtStart)
                yield return extraElement;
            foreach (var e in source)
                yield return e;
            if (!insertAtStart)
                yield return extraElement;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return Shuffle(source, new System.Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, System.Random rng)
        {
            var elements = source.ToArray();

            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        /// <summary> 
        /// Returns the index of the first element in this <paramref name="source"/> 
        /// satisfying the specified <paramref name="condition"/>. If no such elements 
        /// are found, returns -1. 
        /// </summary> 
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (condition == null)
                throw new ArgumentNullException("condition");
            int index = 0;
            foreach (var v in source)
            {
                if (condition(v))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>Returns the first element from the input sequence for which the 
        /// value selector returns the smallest value.</summary> 
        public static T MinElement<T, TValue>(this IEnumerable<T> source,
                Func<T, TValue> valueSelector) where TValue : IComparable<TValue>
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("source contains no elements.");
                T minElem = enumerator.Current;
                TValue minValue = valueSelector(minElem);
                while (enumerator.MoveNext())
                {
                    TValue value = valueSelector(enumerator.Current);
                    if (value.CompareTo(minValue) < 0)
                    {
                        minValue = value;
                        minElem = enumerator.Current;
                    }
                }
                return minElem;
            }
        }

        // Method to AddRange where items match a given predicate - JHE
        public static void AddRangeWhere<T>(this IEnumerable<T> list, IEnumerable<T> sourceList, Func<T, bool> predicate)
        {
            if (list != null)
            {
                List<T> newRange = new List<T>();

                foreach (var item in sourceList)
                {
                    if (predicate(item))
                        newRange.Add(item);
                }

                list.Concat(newRange);
            }
        }


        #region Sort & Remove methods - JHE
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> list) where T : struct
        {
            return list.OrderBy(i => i);
        }
        public static IEnumerable<string> Sort(this IEnumerable<string> list)
        {
            return list.OrderBy(i => i);
        }
        public static IEnumerable<string> SortAndRemoveDuplicates(this IEnumerable<string> list)
        {
            return list.Sort().Distinct();
        }
        #endregion

        /// <summary>
        /// Returns the IDs specified in the propertySelector of items in the list having more than 1 item (propertySelector) in list with same value. - JHE
        /// Example: 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TProp> SelectDuplicatesObjectIDs<T, TProp>(this IEnumerable<T> list, Func<T, TProp> propertySelector)
        {
            var listCount = list.GroupBy(d => propertySelector(d)).Select(g => new
            {
                ID = g.Key,
                Total = g.Count()
            });
            return listCount.Where(r => r.Total > 1).Select(r => r.ID).ToList();
        }

        public static bool AreAllHashValuesUnique<T>(this IEnumerable<T> list, IEqualityComparer<T> equalityComparer)
        {
            Dictionary<int, T> sourceItems = new Dictionary<int, T>();
            foreach (var item in list)
            {
                var hashValue = equalityComparer.GetHashCode(item);
                if (!sourceItems.ContainsKey(hashValue))
                    sourceItems.Add(hashValue, item);
                else
                    return false;
            }
            return true;
        }

        public static Type GetTypeCheckSingle<T>(this IEnumerable<T> list)
        {
            Type type = null;
            if (list != null)
            {
                var first = list.FirstOrDefault();
                if (first != null)
                    type = first.GetType();
            }
            if (type == null)
                type = typeof(T);

            return type;
        }

        public static IList ToGenericList(this IEnumerable<dynamic> list)
        {
            Type type = list.GetTypeCheckSingle();
            IList newList = NetSteps.Common.Reflection.Reflection.CreateGenericListofType(type) as IList;
            foreach (var item in list)
                newList.Add(item);

            return newList;
        }


        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source.Where((x, i) => i % chunkSize == 0).Select((x, i) => source.Skip(i * chunkSize).Take(chunkSize));
        }

        /// <summary>
        /// Splits a sequence of values based on a predicate.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> SplitWhere<T>(this IEnumerable<T> source, Func<T, T, bool> firstSecondPredicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (firstSecondPredicate == null)
            {
                throw new ArgumentNullException("firstSecondPredicate");
            }
            if (!source.Any())
            {
                yield break;
            }

            var buffer = new List<T>();
            var previousElement = source.First();
            buffer.Add(previousElement);

            foreach (T element in source.Skip(1))
            {
                if (firstSecondPredicate(previousElement, element))
                {
                    yield return buffer.ToArray();
                    buffer.Clear();
                }
                buffer.Add(element);
                previousElement = element;
            }

            if (buffer.Any())
                yield return buffer.ToArray();
        }

        /// <summary>
        /// Returns the maximum number of consecutive months appearing in a sequence of dates.
        /// </summary>
        public static int MaxConsecutiveMonths(this IEnumerable<DateTime> dates)
        {
            if (dates == null)
            {
                throw new ArgumentNullException("dates");
            }

            return dates
                .Select(x => x.TotalMonths())
                .MaxConsecutive();
        }

        /// <summary>
        /// Returns the maximum number of distinct, consecutive numbers appearing in a sequence.
        /// </summary>
        public static int MaxConsecutive(this IEnumerable<int> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (!values.Any())
            {
                return 0;
            }

            return values
                .Distinct()
                .OrderBy(x => x)
                .SplitWhere((first, second) => second != first + 1)
                .Max(x => x.Count());
        }

        /// <summary>
        /// Recursive query operator
        /// </summary>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (T item in source)
            {
                yield return item;

                IEnumerable<T> seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }
    }
}
