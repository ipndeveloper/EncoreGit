using NetSteps.Common.Base;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// Caches order search results
	/// </summary>
	public interface IOrderSearchCache
	{
		bool TryRemove(OrderSearchParameters key, out PaginatedList<OrderSearchData> removed);
		bool TryGet(OrderSearchParameters key, out PaginatedList<OrderSearchData> value);
		PaginatedList<OrderSearchData> Search(OrderSearchParameters key);
	}

    [ContainerRegister(typeof(IOrderSearchCache), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class OrderSearchCache : IOrderSearchCache
	{
		static readonly string CacheName = "Order Search";
		
		ICache<OrderSearchParameters, PaginatedList<OrderSearchData>> _cache;

		public OrderSearchCache() : this(null)
		{
		}
		
		public OrderSearchCache(ICacheItemResolver<OrderSearchParameters, PaginatedList<OrderSearchData>> resolver)			
		{
			var localResolver = resolver ?? new OrderSearchResolver();
			_cache = new ActiveMruLocalMemoryCache<OrderSearchParameters, PaginatedList<OrderSearchData>>(CacheName, localResolver);
		}

		/// <summary>
		/// Tries to remove an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="removed">variable where the removed item will be returned upon success</param>
		/// <returns>true if the item was removed; otherwise false</returns>
		public bool TryRemove(OrderSearchParameters key, out PaginatedList<OrderSearchData> removed)
		{
			return _cache.TryRemove(key, out removed);
		}

		/// <summary>
		/// Tries to get an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item was retrieved; otherwise false</returns>
		public bool TryGet(OrderSearchParameters key, out PaginatedList<OrderSearchData> value)
		{
			return _cache.TryGet(key, out value);
		}

		public PaginatedList<OrderSearchData> Search(OrderSearchParameters key)
		{
			PaginatedList<OrderSearchData> orders;
			if (!TryGet(key, out orders))
				orders = new PaginatedList<OrderSearchData>();
			return orders;
		}
	}
}
