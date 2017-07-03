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
    public class ContinuityQualificationHandler : BasePromotionQualificationHandler<IContinuityQualificationExtension, IContinuityQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IContinuityQualificationHandler
    {
        public ContinuityQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.ContinuityProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext order)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(promotionQualification is IContinuityQualificationExtension);
            Contract.Assert(order != null);
            if (order.CurrentActivity != null)
            {
                IContinuityQualificationExtension qualification = (IContinuityQualificationExtension)promotionQualification;
                if (qualification.HasContinuity && order.CurrentActivity.HasContinuity)
                    return PromotionQualificationResult.MatchForAllCustomers;
                return PromotionQualificationResult.NoMatch;
            }
            else
                return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IContinuityQualificationExtension);
            Contract.Assert(promotionQualification2 is IContinuityQualificationExtension);

            var extension1 = promotionQualification1 as IContinuityQualificationExtension;
            var extension2 = promotionQualification2 as IContinuityQualificationExtension;

            return extension1.HasContinuity.Equals(extension2.HasContinuity);
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }



        public override void CheckValidity(string qualificationKey, IContinuityQualificationExtension qualification, IPromotionState state)
		{
			
		}
	}
}
