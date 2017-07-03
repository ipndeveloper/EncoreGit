using System;
using System.Collections.Generic;

namespace NetSteps.Silverlight.Comparer
{
    // http://brendan.enrick.com/blog/linq-your-collections-with-iequalitycomparer-and-lambda-expressions/ - JHE
    /// <summary>
    /// Example:
    /// var f3 = album.AlbumImages.Except(siteAlbum.Album.AlbumImages, new LambdaComparer<AlbumImage>((x, y) => x.Image.ImageUrl == y.Image.ImageUrl));
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaComparer;
        private readonly Func<T, int> _lambdaHash;

        public LambdaComparer(Func<T, T, bool> lambdaComparer)
            : this(lambdaComparer, o => o.GetHashCode())
        {
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

        public bool IsLambdaHashMethodSet()
        {
            return (_lambdaHash != null);
        }
    }
}
