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
    public class OrderTypeQualificationHandler : BasePromotionQualificationHandler<IOrderTypeQualificationExtension, IOrderTypeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IOrderTypeQualificationHandler
    {
        public OrderTypeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.OrderTypeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IOrderTypeQualificationExtension);

            IOrderTypeQualificationExtension qualification = (IOrderTypeQualificationExtension)promotionQualification;
            if (qualification.OrderTypes.Contains(orderContext.Order.OrderTypeID))
                return PromotionQualificationResult.MatchForAllCustomers;
            return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IOrderTypeQualificationExtension);
            Contract.Assert(promotionQualification2 is IOrderTypeQualificationExtension);

            var extension1 = promotionQualification1 as IOrderTypeQualificationExtension;
            var extension2 = promotionQualification2 as IOrderTypeQualificationExtension;

            if (extension1.OrderTypes.Except(extension2.OrderTypes).Count() > 0)
                return false;
            if (extension2.OrderTypes.Except(extension1.OrderTypes).Count() > 0)
                return false;
            return true;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, IOrderTypeQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.OrderTypes == null || !qualification.OrderTypes.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Order types collection is null or empty."
					);
			}
		}
	}
}
