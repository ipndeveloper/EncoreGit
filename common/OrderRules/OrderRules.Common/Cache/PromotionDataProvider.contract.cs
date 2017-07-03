using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.OrderRules.Common.Model;

namespace NetSteps.OrderRules.Common.Cache
{
	[ContractClassFor(typeof(IPromotionDataProvider))]
	public abstract class PromotionDataProviderContract : IPromotionDataProvider
	{
		public IPromotion FindPromotion(int promotionID, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "FindPromotion requires a non-null unitOfWork.");
			Contract.Requires<ArgumentOutOfRangeException>(promotionID > 0, "FindPromotion requires promotionID > 0.");
			throw new NotImplementedException();
		}

		public IEnumerable<IPromotion> FindPromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
			Contract.Requires<ArgumentNullException>(ofKinds != null);
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "FindPromotions requires a non-null unitOfWork.");
			throw new NotImplementedException();
		}

		public int FindPromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID)
		{
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "FindPromotionIDByPromotionQualificationID requires a non-null unitOfWork.");
			Contract.Requires<ArgumentOutOfRangeException>(promotionQualificationID > 0, "FindPromotionIDByPromotionQualificationID requires promotionQualificationID > 0.");
			throw new NotImplementedException();
		}

		public IPromotion AddPromotion(IPromotion promotion, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentNullException>(promotion != null, "AddPromotion requires a non-null promotion.");
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "AddPromotion requires a non-null unitOfWork.");
			throw new NotImplementedException();
		}

		public IPromotion UpdatePromotion(IPromotion promotion, Data.Common.IUnitOfWork unitOfWork)
		{
			Contract.Requires<ArgumentNullException>(promotion != null, "UpdatePromotion requires a non-null promotion.");
			Contract.Requires<ArgumentNullException>(unitOfWork != null, "UpdatePromotion requires a non-null unitOfWork.");
			Contract.Requires<ArgumentOutOfRangeException>(promotion.PromotionID > 0, "UpdatePromotion requires promotion with PromotionID > 0.");
			throw new NotImplementedException();
		}
	}
}
