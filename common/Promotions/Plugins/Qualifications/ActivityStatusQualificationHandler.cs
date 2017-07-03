using System;
using System.Linq;
using NetSteps.Promotions.Common.Model;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;


namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class ActivityStatusQualificationHandler : BasePromotionQualificationHandler<IActivityStatusQualificationExtension, IActivityStatusQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IActivityStatusQualificationHandler
    {
        public ActivityStatusQualificationHandler()
        {
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IActivityStatusQualificationExtension);

            if (orderContext.CurrentActivity != null)
            {
                IActivityStatusQualificationExtension qualification = (IActivityStatusQualificationExtension)promotionQualification;
                if (qualification.ActivityStatuses.Contains(orderContext.CurrentActivity.ActivityStatusID))
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
            Contract.Assert(promotionQualification1 is IActivityStatusQualificationExtension);
            Contract.Assert(promotionQualification2 is IActivityStatusQualificationExtension);

            var extension1 = promotionQualification1 as IActivityStatusQualificationExtension;
            var extension2 = promotionQualification2 as IActivityStatusQualificationExtension;

            if (extension1.ActivityStatuses.Except(extension2.ActivityStatuses).Count() > 0)
                return false;
            if (extension2.ActivityStatuses.Except(extension1.ActivityStatuses).Count() > 0)
                return false;
            return true;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.ActivityStatusProviderKey;
        }

        public override void CheckValidity(string qualificationKey, IActivityStatusQualificationExtension qualification, IPromotionState state)
		{
            if (qualification.ActivityStatuses == null || !qualification.ActivityStatuses.Any())
			{
				state.AddConstructionError
					(
						String.Format("Activity Status Qualification {0}", qualificationKey),
						"Activity Statuses collection is null or empty."
					);
			}
		}
	}
}
