using System;
using System.Collections.Generic;

namespace NetSteps.Common.Comparer
{
	/// <summary>
	/// Author: John Egbert
	/// Description: This was taken from http://brendan.enrick.com/blog/linq-your-collections-with-iequalitycomparer-and-lambda-expressions/
	/// Example Usage: var f3 = album.AlbumImages.Except(siteAlbum.Album.AlbumImages, new LambdaComparer{AlbumImage}((x, y) => x.Image.ImageUrl == y.Image.ImageUrl));
	/// Created: 03-11-2010
	/// </summary>
	public class LambdaComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _lambdaComparer;
		private readonly Func<T, int> _lambdaHash;

		public LambdaComparer(Func<T, T, bool> lambdaComparer)
			: this(lambdaComparer, o => o.GetHashCode())
		{
            // Note: When using this constructor, the lambdaComparer and lambdaHash are not
            // consistent with one another. In some cases (i.e. Except() and Distinct()), this
            // may cause undesired behavior.
            // For example:
            //     var matches = Regex.Match(input, pattern);
            //     matches.Cast<Match>().Distinct((m1, m2) => m1.Value == m2.Value);
            // Distinct() will call GetHashCode() on each Match object and find it to be unique.
            // It will never do a compare on Match.Value so it will never remove any regex matches,
            // even though some may have duplicate values.
            // 
            // Consider using the LambdaEqualityComparer class below instead. It keeps the
            // Equals() and GetHashCode() methods consistent with one another. - JGL
		}

		public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
		{
			if (lambdaComparer == null)
				throw new ArgumentNullException("lambdaComparer");
			if (lambdaHash == null)
				throw new ArgumentNullException("lambdaHash");

			_lambdaComparer = lambdaComparer;
			_lambdaHash = lambdaHash;
		}

		public bool Equals(T x, T y)
		{
			return _lambdaComparer(x, y);
		}

		public int GetHashCode(T obj)
		{
			return _lambdaHash(obj);
		}

		//public bool IsLambdaHashMethodSet()
		//{
		//    return (_lambdaHash != null);
		//}
	}

    #region LambdaEqualityComparer helper methods for type inference
    public static class LambdaEqualityComparer
    {
        public static LambdaEqualityComparer<T, TKey> Create<T, TKey>(Func<T, TKey> keySelector)
        {
            return new LambdaEqualityComparer<T, TKey>(keySelector);
        }
        public static LambdaEqualityComparer<T, TKey> Create<T, TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new LambdaEqualityComparer<T, TKey>(keySelector, comparer);
        }
    }
    public static class LambdaEqualityComparer<T>
    {
        public static LambdaEqualityComparer<T, TKey> Create<TKey>(Func<T, TKey> keySelector)
        {
            return new LambdaEqualityComparer<T, TKey>(keySelector);
        }
        public static LambdaEqualityComparer<T, TKey> Create<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new LambdaEqualityComparer<T, TKey>(keySelector, comparer);
        }
    }
    #endregion
    /// <summary>
    /// Comparer which projects each element of the comparison to a key, and then compares
    /// those keys using the specified (or default) comparer for the key type.
    /// Taken from Jon Skeet's ProjectionEqualityComparer example:
    /// http://stackoverflow.com/questions/188120/can-i-specify-my-explicit-type-comparator-inline/188130#188130
    /// </summary>
    /// <typeparam name="T">Type of elements which this comparer will be asked to compare</typeparam>
    /// <typeparam name="TKey">Type of the key projected from the element</typeparam>
    public class LambdaEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _comparer;

        /// <summary>
        /// Creates a new instance using the specified lambda expression, which must not be null.
        /// The default comparer for the key type is used.
        /// </summary>
        /// <param name="keySelector">Lambda expression to use during comparisons</param>
        public LambdaEqualityComparer(Func<T, TKey> keySelector) : this(keySelector, null) { }

        /// <summary>
        /// Creates a new instance using the specified lambda expression, which must not be null.
        /// </summary>
        /// <param name="keySelector">Lambda expression to use during comparisons</param>
        /// <param name="comparer">The comparer to use on the keys. May be null,
        /// in which case the default comparer will be used.</param>
        public LambdaEqualityComparer(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            _keySelector = keySelector;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}
