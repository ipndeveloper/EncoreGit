using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Promotions.Common.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common;

namespace NetSteps.Promotions.Common.CoreImplementations
{
	public class NonCachingPromotionDataProvider : IPromotionDataProvider
	{
        private readonly IPromotionRepository _repository;

        public NonCachingPromotionDataProvider(IPromotionRepository repository)
        {
            _repository = repository;
        }

		/// <summary>
		/// Finds the promotion.
		/// </summary>
		/// <param name="promotionID">The promotion ID.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		public Model.IPromotion FindPromotion(int promotionID, Data.Common.IUnitOfWork unitOfWork)
		{
			return _repository.RetrievePromotion(promotionID, unitOfWork);
		}

		public IEnumerable<Model.IPromotion> FindPromotions(IPromotionUnitOfWork unitOfWork, Model.PromotionStatus statusTypes, IPromotionInterval searchInterval, Predicate<Model.IPromotion> filter, IEnumerable<string> ofKinds)
		{
			return _repository.RetrievePromotions(unitOfWork, statusTypes, filter, ofKinds);
		}

		/// <summary>
		/// Finds the promotion ID by promotion qualification ID.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="promotionQualificationID">The promotion qualification ID.</param>
		/// <returns></returns>
		public int FindPromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID)
		{
			return _repository.RetrievePromotionIDByPromotionQualificationID(unitOfWork, promotionQualificationID);
		}

		/// <summary>
		/// Adds the promotion.
		/// </summary>
		/// <param name="promotion">The promotion.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		public IPromotion AddPromotion(IPromotion promotion, IUnitOfWork unitOfWork)
		{
            return _repository.InsertPromotion(promotion, unitOfWork);
		}

		/// <summary>
		/// Updates the promotion.
		/// </summary>
		/// <param name="promotion">The promotion.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		public IPromotion UpdatePromotion(IPromotion promotion, IUnitOfWork unitOfWork)
		{
			return _repository.UpdateExistingPromotion(promotion, unitOfWork);
		}
	}
}
