using NetSteps.Common.Base;
using NetSteps.Core.Cache;

namespace NetSteps.Data.Entities.Business
{
	internal sealed class OrderSearchResolver : CacheItemResolver<OrderSearchParameters, PaginatedList<OrderSearchData>>
	{
		protected override ResolutionKind PerformTryResolve(OrderSearchParameters key, out PaginatedList<OrderSearchData> value)
		{
			value = Order.Search(key);
			return ResolutionKind.Created;
		}
	}
}
