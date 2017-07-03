using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// Represents a generic collection of key/value pairs where the values are lazy-initialized.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of lazy-initialized values in the dictionary.</typeparam>
    public interface ILazyDictionary<TKey, TValue> : IDictionary<TKey, Lazy<TValue>>, ICollection<KeyValuePair<TKey, Lazy<TValue>>>, IEnumerable<KeyValuePair<TKey, Lazy<TValue>>>, IEnumerable
    {
        /// <summary>
        /// Gets the lazy-initialized value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements <see cref="ILazyDictionary{TKey,TValue}"/> contains an element with the specified key; otherwise, false.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Gets an <see cref="ICollection{TValue}"/> containing the lazy-initialized values in the <see cref="ILazyDictionary{TKey,TValue}"/>.
        /// </summary>
        new ICollection<TValue> Values { get; }

        /// <summary>
        /// Gets the lazy-initialized element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>The lazy-initialized element with the specified key.</returns>
        new TValue this[TKey key] { get; }
    }

    /// <summary>
    /// Represents a generic collection of key/value pairs where the values are lazy-initialized.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of lazy-initialized values in the dictionary.</typeparam>
    public class LazyDictionary<TKey, TValue> : Dictionary<TKey, Lazy<TValue>>, ILazyDictionary<TKey, TValue>, IDictionary<TKey, Lazy<TValue>>, ICollection<KeyValuePair<TKey, Lazy<TValue>>>, IEnumerable<KeyValuePair<TKey, Lazy<TValue>>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public LazyDictionary() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="LazyDictionary{TKey,TValue}"/> can contain.</param>
        public LazyDictionary(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that is empty, has the default initial capacity, and uses the specified <see cref="System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> implementation to use when comparing keys, or null to use the default <see cref="System.Collections.Generic.EqualityComparer{T}"/> for the type of the key.</param>
        public LazyDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="LazyDictionary{TKey,TValue}"/> can contain.</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> implementation to use when comparing keys, or null to use the default <see cref="System.Collections.Generic.EqualityComparer{T}"/> for the type of the key.</param>
        public LazyDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that contains elements copied from the specified <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> whose elements are copied to the new <see cref="LazyDictionary{TKey,TValue}"/>.</param>
        public LazyDictionary(IDictionary<TKey, Lazy<TValue>> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that contains elements copied from the specified <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> and uses the specified <see cref="System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="System.Collections.Generic.IDictionary{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> whose elements are copied to the new <see cref="LazyDictionary{TKey,TValue}"/>.</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> implementation to use when comparing keys, or null to use the default <see cref="System.Collections.Generic.EqualityComparer{T}"/> for the type of the key.</param>
        public LazyDictionary(IDictionary<TKey, Lazy<TValue>> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class that contains elements copied from the specified <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where TValue is a <see cref="Lazy{TValue}"/> whose elements are added to the new <see cref="LazyDictionary{TKey,TValue}"/>.</param>
        public LazyDictionary(IEnumerable<KeyValuePair<TKey, Lazy<TValue>>> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var keyValuePair in collection)
            {
                base.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyDictionary{TKey,TValue}"/> class with serialized data.
        /// </summary>
        /// <param name="info">A System.Runtime.Serialization.SerializationInfo object containing the information required to serialize the <see cref="LazyDictionary{TKey,TValue}"/>.</param>
        /// <param name="context">A System.Runtime.Serialization.StreamingContext structure containing the source and destination of the serialized stream associated with the <see cref="LazyDictionary{TKey,TValue}"/>.</param>
        protected LazyDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Gets the lazy-initialized value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the <see cref="LazyDictionary{TKey,TValue}"/> contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            Lazy<TValue> lazyValue;
            bool success = base.TryGetValue(key, out lazyValue);
            if (success)
            {
                value = lazyValue.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Gets an <see cref="ICollection{TValue}"/> containing the lazy-initialized values in the <see cref="LazyDictionary{TKey,TValue}"/>.
        /// </summary>
        new public ICollection<TValue> Values
        {
            get
            {
                return new ReadOnlyCollection<TValue>(
                    base.Values
                        .Select(x => x.Value)
                        .ToList()
                );
            }
        }

        /// <summary>
        /// Gets the lazy-initialized element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>The lazy-initialized element with the specified key.</returns>
        new public TValue this[TKey key]
        {
            get { return base[key].Value; }
        }
    }
}
