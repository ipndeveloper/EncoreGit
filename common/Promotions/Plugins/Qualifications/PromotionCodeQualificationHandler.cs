using System;
using System.Linq;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;


namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class PromotionCodeQualificationHandler : BasePromotionQualificationHandler<IPromotionCodeQualificationExtension, IPromotionCodeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionCodeQualificationHandler
    {
        public PromotionCodeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.PromotionCodeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext order)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(promotionQualification != null);
            Contract.Assert(promotionQualification is IPromotionCodeQualificationExtension);
            Contract.Assert(order != null);

            IPromotionCodeQualificationExtension qualification = (IPromotionCodeQualificationExtension)promotionQualification;
            if (order.CouponCodes != null)
			{
				var customerIDs = order.CouponCodes.Where(couponCode => couponCode.CouponCode.Equals(qualification.PromotionCode, StringComparison.InvariantCultureIgnoreCase))
					.Select(couponCode => couponCode.AccountID);
				if (customerIDs.Count() == order.Order.OrderCustomers.Count)
					return PromotionQualificationResult.MatchForAllCustomers;
				else
					return PromotionQualificationResult.MatchForSelectCustomers(customerIDs);
			}
            return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IPromotionCodeQualificationExtension);
            Contract.Assert(promotionQualification2 is IPromotionCodeQualificationExtension);

            var extension1 = promotionQualification1 as IPromotionCodeQualificationExtension;
            var extension2 = promotionQualification2 as IPromotionCodeQualificationExtension;

            return extension1.PromotionCode.Equals(extension2.PromotionCode, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }



		public override void CheckValidity(string qualificationKey, IPromotionCodeQualificationExtension qualification, IPromotionState state)
		{
			if (String.IsNullOrEmpty(qualification.PromotionCode))
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Promotion code is null or empty."
					);
			}
		}
	}
}
