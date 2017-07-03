using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Dto;

using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Cache;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;

namespace NetSteps.Promotions.Caching
{
	public class CachingPromotionDataProvider : IPromotionDataProvider
	{
		private readonly ICacheMany<int, IPromotion> promotionCache;
        private readonly IPromotionRepository _promotionRepository;

		public CachingPromotionDataProvider(IPromotionRepository promotionRepository)
		{
			promotionCache = new ActiveMruLocalMemoryCache<int, IPromotion>("Promotions", new PromotionCacheResolver());
            _promotionRepository = promotionRepository;
		}

		public IPromotion AddPromotion(IPromotion promotion, IUnitOfWork unitOfWork)
		{
			using (var ctx = Create.SharedOrNewContainer())
			{
                var savedPromotion = _promotionRepository.InsertPromotion(promotion, unitOfWork);
				Contract.Assert(savedPromotion.PromotionID > 0);
				promotionCache.TryAdd(savedPromotion.PromotionID, savedPromotion.Clone());
				return savedPromotion.Clone();
			}
		}

		public IPromotion FindPromotion(int promotionID, IUnitOfWork unitOfWork)
		{
			IPromotion found = null;
			if (promotionCache.TryGet(promotionID, out found))
			{
				return found.Clone();
			}
			else
			{
				return null;
			}
		}

		public int FindPromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID)
		{
            return _promotionRepository.RetrievePromotionIDByPromotionQualificationID(unitOfWork, promotionQualificationID);
		}

		public IEnumerable<IPromotion> FindPromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
            var promotionIDList = _promotionRepository.RetrievePromotionIDs(unitOfWork, statusTypes, searchInterval, ofKinds);
			IEnumerable<IPromotion> promotions;
			if (promotionIDList.Any() && promotionCache.TryGetAny(promotionIDList, out promotions))
			{
				return promotions.Where((x) => filter(x)).Select(x => x.Clone());
			}
			else
			{
				return Enumerable.Empty<IPromotion>();
			}
		}

		public IPromotion UpdatePromotion(IPromotion promotion, IUnitOfWork unitOfWork)
		{
            var savedPromotion = _promotionRepository.UpdateExistingPromotion(promotion, unitOfWork);
			IPromotion removed;
			promotionCache.TryRemove(promotion.PromotionID, out removed);
			promotionCache.TryAdd(savedPromotion.PromotionID, savedPromotion.Clone());
			return savedPromotion.Clone();
		}

	}
}
