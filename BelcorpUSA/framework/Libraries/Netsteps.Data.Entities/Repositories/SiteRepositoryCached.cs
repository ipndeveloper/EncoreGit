using System;
using System.Diagnostics.Contracts;
using NetSteps.Core.Cache;
using NetSteps.Sites.Common.Models;

namespace NetSteps.Data.Entities.Repositories
{
	/// <summary>
	/// Cached implementation of <see cref="SiteRepository"/>.
	/// </summary>
	public class SiteRepositoryCached : SiteRepository
	{
		private readonly ICache<int, ISite> _siteCache;
		private readonly ICache<string, Tuple<int?>> _urlSiteLookupCache;

		public SiteRepositoryCached(
			ICache<int, ISite> siteCache,
			ICache<string, Tuple<int?>> urlSiteLookupCache)
		{
			Contract.Requires<ArgumentNullException>(siteCache != null);
			Contract.Requires<ArgumentNullException>(urlSiteLookupCache != null);

			_siteCache = siteCache;
			_urlSiteLookupCache = urlSiteLookupCache;
		}

		/// <summary>
		/// Cached!
		/// </summary>
		public override ISite GetSite(int siteId)
		{
			return _siteCache.GetOrAdd(siteId, base.GetSite);
		}

		/// <summary>
		/// Cached!
		/// </summary>
		public override int? GetSiteId(string url)
		{
			// All of this is to avoid caching nulls.
			var wrappedSiteId = _urlSiteLookupCache.GetOrAdd(url, key =>
			{
				var siteId = base.GetSiteId(key);
				return siteId.HasValue
					? Tuple.Create(siteId)
					: default(Tuple<int?>);
			});
			return wrappedSiteId == default(Tuple<int?>)
				? null
				: wrappedSiteId.Item1;
		}
	}

	/// <summary>
	/// Putting this here temporarily until we decide if it's going to stay.
	/// </summary>
	public static class ICacheExtensions
	{
		/// <summary>
		/// Gets an item's representation from the cache by the item's key,
		/// or resolves the item using the specified function and adds it
		/// to the cache.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="key">the item's key</param>
		/// <param name="resolver">the function used to generate a representation for the key</param>
		/// <returns>the item's representation</returns>
		public static R GetOrAdd<K, R>(this ICache<K, R> cache, K key, Func<K, R> resolver)
			where R : class
		{
			Contract.Requires<ArgumentNullException>(cache != null);
			Contract.Requires<ArgumentNullException>(resolver != null);

			R representation;
			if (!cache.TryGet(key, out representation))
			{
				representation = resolver(key);
				// Don't cache nulls
				if (representation != default(R))
				{
					cache.TryAdd(key, representation);
				}
			}
			return representation;
		}

        /// <summary>
        /// Resolves the item using the specified function and adds it,
        /// or updates the existing item to the cache.
        /// </summary>
        /// <typeparam name="K">key type K</typeparam>
        /// <typeparam name="R">representation type R</typeparam>
        /// <param name="cache">the cache</param>
        /// <param name="key">the item's key</param>
        /// <param name="resolver">the function used to generate a representation for the key</param>
        /// <param name="resolver">the function used to update a representation for the key</param>
        /// <returns>the item's new representation</returns>
        public static R AddOrUpdate<K, R>(this ICache<K, R> cache, K key, Func<K, R> resolver, Func<R, R> updater)
            where R : class
        {
            Contract.Requires<ArgumentNullException>(cache != null);
            Contract.Requires<ArgumentNullException>(resolver != null);

            R representation;
            if (!cache.TryGet(key, out representation))
            {
                representation = resolver(key);
                // Don't cache nulls
                if (representation != default(R))
                {
                    cache.TryAdd(key, representation);
                }
            }
            else
            {
                var updated = updater(representation);

                if (representation != default(R))
                {
                    if (cache.TryUpdate(key, updated, representation))
                    {
                        representation = updated;
                    }
                }
            }
            return representation;
        }
	}
}
