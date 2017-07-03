using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Core.Cache;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.Caching
{
	public class PromotionCacheResolver : DemuxCacheItemResolver<int, IPromotion>//DemuxCacheManyItemResolver<int, IPromotion>
	{
		protected override bool DemultiplexedTryResolve(int key, out IPromotion value)
		{
			var unitOfWork = Create.New<IPromotionUnitOfWork>();
			var repository = Create.New<IPromotionRepository>();
			value = repository.RetrievePromotion(key, unitOfWork);
			return value != null;
		}

		//protected override bool DemultiplexedTryResolveMany(IEnumerable<int> keys, out IEnumerable<KeyValuePair<int, IPromotion>> values)
		//{
		//    var unitOfWork = Create.New<IPromotionUnitOfWork>();
		//    var repository = Create.New<IPromotionRepository>();
		//    values = repository.RetrievePromotions(unitOfWork, PromotionStatus.Enabled, p => keys.Contains(p.PromotionID), Enumerable.Empty<string>()).Select(p => new KeyValuePair<int, IPromotion>(p.PromotionID, p)).ToArray();
		//    return values != null && (values.Count() == keys.Count());
		//}
	}
}
