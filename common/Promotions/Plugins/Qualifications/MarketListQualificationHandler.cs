using System;
using System.Linq;
using NetSteps.Promotions.Common.Model;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;


namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class MarketListQualificationHandler : BasePromotionQualificationHandler<IMarketListQualificationExtension, IMarketListQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IMarketListQualificationHandler
    {
        public MarketListQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.MarketListProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IMarketListQualificationExtension);

            IMarketListQualificationExtension qualification = (IMarketListQualificationExtension)promotionQualification;
            if (qualification.Markets.Contains(orderContext.Order.GetShippingMarketID()))
                return PromotionQualificationResult.MatchForAllCustomers;
            return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IMarketListQualificationExtension);
            Contract.Assert(promotionQualification2 is IMarketListQualificationExtension);

            var extension1 = promotionQualification1 as IMarketListQualificationExtension;
            var extension2 = promotionQualification2 as IMarketListQualificationExtension;

            if (extension1.Markets.Except(extension2.Markets).Count() > 0)
                return false;
            if (extension2.Markets.Except(extension1.Markets).Count() > 0)
                return false;
            return true;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, IMarketListQualificationExtension qualification, IPromotionState state)
		{
			// doesn't verify if the markets are actually valid....
			if (qualification.Markets == null || !qualification.Markets.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Market collection is null or empty."
					);
			}
		}
	}
}
