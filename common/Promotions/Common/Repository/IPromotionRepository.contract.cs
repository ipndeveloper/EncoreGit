using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.Repository
{
	[ContractClassFor(typeof(IPromotionRepository))]
	public abstract class PromotionRepositoryContract : IPromotionRepository
	{
		public IPromotion InsertPromotion(IPromotion promotion, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentNullException>(promotion != null, "InsertPromotion requires a non-null promotion.");
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "InsertPromotion requires a non-null unitOfWork.");
			Contract.Ensures(Contract.Result<IPromotion>().PromotionID != 0);
			throw new NotImplementedException();
		}

		public IPromotion UpdateExistingPromotion(IPromotion promotion, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentNullException>(promotion != null, "UpdateExistingPromotion requires a non-null promotion.");
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "UpdateExistingPromotion requires a non-null unitOfWork.");
			Contract.Requires<ArgumentOutOfRangeException>(promotion.PromotionID > 0, "UpdateExistingPromotion requires promotion with PromotionID > 0.");
			throw new NotImplementedException();
		}

		public IPromotion RetrievePromotion(int promotionID, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentOutOfRangeException>(promotionID > 0, "RetrievePromotion requires promotionID > 0.");
			throw new NotImplementedException();
		}

		public IEnumerable<IPromotion> RetrievePromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
			Contract.Requires<ArgumentNullException>(ofKinds != null);
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "RetrievePromotions requires a non-null unitOfWork.");
			throw new NotImplementedException();
		}

		public int RetrievePromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID)
		{
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "RetrievePromotionIDByPromotionQualificationID requires a non-null unitOfWork.");
			Contract.Requires<ArgumentOutOfRangeException>(promotionQualificationID > 0, "RetrievePromotionIDByPromotionQualificationID requires promotionQualificationID > 0.");
			throw new NotImplementedException();
		}

		public IEnumerable<int> RetrievePromotionIDs(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, IEnumerable<string> ofKinds)
		{
			Contract.Requires<ArgumentNullException>(ofKinds != null);
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "RetrievePromotionIDs requires a non-null unitOfWork.");
			throw new NotImplementedException();
		}
	}
}
