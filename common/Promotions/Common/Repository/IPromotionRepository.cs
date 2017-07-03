using System;
using System.Collections.Generic;
using NetSteps.Data.Common;
using NetSteps.Promotions.Common.Model;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common.Repository
{
    /// <summary>
    /// Promotion repository with matching.
    /// </summary>
	[ContractClass(typeof(PromotionRepositoryContract))]
	public interface IPromotionRepository
	{
		/// <summary>
		/// Inserts the promotion.
		/// </summary>
		/// <param name="promotion">The promotion.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		IPromotion InsertPromotion(IPromotion promotion, IUnitOfWork unitOfWork);

		/// <summary>
		/// Updates an existing promotion.
		/// </summary>
		/// <param name="promotion">The promotion.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		IPromotion UpdateExistingPromotion(IPromotion promotion, IUnitOfWork unitOfWork);

		/// <summary>
		/// Retrieves the promotion.
		/// </summary>
		/// <param name="promotionID">The promotion ID.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <returns></returns>
		IPromotion RetrievePromotion(int promotionID, IUnitOfWork unitOfWork);

		/// <summary>
		/// Retrieves the promotions.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="statusTypes">The status types.</param>
		/// <param name="filter">The filter.</param>
		/// <param name="ofKinds">The of kinds.</param>
		/// <returns></returns>
		IEnumerable<IPromotion> RetrievePromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, Predicate<IPromotion> filter, IEnumerable<string> ofKinds);
		
		/// <summary>
		/// Retrieves the promotion ID by a child promotion qualification ID.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="promotionQualificationID">The promotion qualification ID.</param>
		/// <returns></returns>
		int RetrievePromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID);

		/// <summary>
		/// Retrieves the promotion Ids.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="statusTypes">The status types.</param>
		/// <param name="searchInterval">The search interval.</param>
		/// <param name="ofKinds">The of kinds.</param>
		/// <returns></returns>
		IEnumerable<int> RetrievePromotionIDs(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, IEnumerable<string> ofKinds);
		
		
	}
}
