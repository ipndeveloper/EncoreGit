using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Extensions;
using System.Collections.Concurrent;

namespace NetSteps.Common.Base
{
    public class IndexedList<V>
    {
        /// <summary>
        /// Cannot Remove any items with this data type.
        /// </summary>
        private readonly BlockingCollection<V> _list;
        
       
        /// <summary>
        /// new Dictionary{string, Func{object}}() { new KeyValuePair{string, Func{V, object}}() { "SKU", (x) => x.SKU }
        /// </summary>
        private IndexedListColumns<V> _indexedListColumns;
        /// <summary>
        /// ConcurrentDictionary{"get_SKU", SortedList{"SKU#", BlockingCollection{Product}}}
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, BlockingCollection<V>>> _indexes = new ConcurrentDictionary<string, ConcurrentDictionary<object, BlockingCollection<V>>>();
        private const BindingFlags _memberFlags = BindingFlags.Public | BindingFlags.Instance;
        
        public IndexedList(List<V> newList, IndexedListColumns<V> indexedListColumns)
        {
            _list = new BlockingCollection<V>(new ConcurrentQueue<V>(newList));
            _indexedListColumns = new IndexedListColumns<V>(indexedListColumns);
            foreach (var column in _indexedListColumns.ToList())
            {
                var newIndex = new ConcurrentDictionary<object, BlockingCollection<V>>();
                foreach (var item in newList)
                    AddItem(newIndex, column, item);

                _indexes.Add(column.Key, newIndex);
            }
        }

        #region Private Methods
        private void AddItem(KeyValuePair<string, Func<V, object>> member, V item)
        {
            AddItem(_indexes[member.Key], member, item);
        }

        //private void removeItem(MemberInfo member, V item)
        //{
        //    removeItem(_indexes[member.Name], member, item);
        //}
        //private void removeItem(ConcurrentDictionary<object, BlockingCollection<V>> index, MemberInfo member, V item)
        //{
        //    object val = GetValue(member, item);
        //    index[val].TryTake(out item);
        //}
        private void AddItem(ConcurrentDictionary<object, BlockingCollection<V>> index, KeyValuePair<string, Func<V, object>> member, V item)
        {
            object key = member.Value(item);
            index.GetOrAdd(key, newKey => new BlockingCollection<V>()).Add(item);
        }

        
        private IEnumerable<string> GetMatches(string[] keys, string fragment)
        {
            int index = Array.BinarySearch(keys, fragment);

            if (index < 0)
            {
                index = ~index;
            }

            var results = new List<string>();
            while (index < keys.Length && keys[index].StartsWith(fragment))
            {
                results.Add(keys[index]);
                index++;
            }

            return results;
        }

        private IndexedListColumns<V> IndexedMembers
        {
            get { return _indexedListColumns; }
        }
        #endregion

        #region Public Methods
        public List<V> List { get { return  _list.ToList(); } }

        public List<V> Get(string column, object value)
        {
            BlockingCollection<V> tmp;
            this._indexes[column].TryGetValue(value, out tmp);
            return tmp == null ? new List<V>() : tmp.ToList();
        }

        public IEnumerable<V> FuzzyGet(string column, string value)
        {
            var keys = this._indexes[column].Keys.Cast<string>().ToArray();

            if (keys.Length == 0)
                return new List<V>();

            var matchingKeys = GetMatches(keys, value.ToUpper());

            var matchingValues = new List<V>();
            foreach (var key in matchingKeys)
            {
                matchingValues.AddRange(this._indexes[column][key]);
            }

            return matchingValues;
        }

        public void Add(V item)
        {
            this._list.Add(item);
            foreach (var column in this.IndexedMembers)
                AddItem(column, item);
        }
        #endregion



    }
}
