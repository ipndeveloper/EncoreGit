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
    public class AccountConsistencyStatusQualificationHandler : BasePromotionQualificationHandler<IAccountConsistencyStatusQualificationExtension, IAccountConsistencyStatusQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IAccountConsistencyStatusQualificationHandler
    {
        public AccountConsistencyStatusQualificationHandler()
        {
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IAccountConsistencyStatusQualificationExtension);

            if (orderContext.CurrentActivity != null)
            {
                IAccountConsistencyStatusQualificationExtension qualification = (IAccountConsistencyStatusQualificationExtension)promotionQualification;
                if (qualification.AccountConsistencyStatuses.Contains(orderContext.CurrentActivity.AccountConsistencyStatusID))
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
            Contract.Assert(promotionQualification1 is IAccountConsistencyStatusQualificationExtension);
            Contract.Assert(promotionQualification2 is IAccountConsistencyStatusQualificationExtension);

            var extension1 = promotionQualification1 as IAccountConsistencyStatusQualificationExtension;
            var extension2 = promotionQualification2 as IAccountConsistencyStatusQualificationExtension;

            if (extension1.AccountConsistencyStatuses.Except(extension2.AccountConsistencyStatuses).Count() > 0)
                return false;
            if (extension2.AccountConsistencyStatuses.Except(extension1.AccountConsistencyStatuses).Count() > 0)
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
            return QualificationExtensionProviderKeys.AccountConsistencyStatusProviderKey;
        }

        public override void CheckValidity(string qualificationKey, IAccountConsistencyStatusQualificationExtension qualification, IPromotionState state)
		{
            if (qualification.AccountConsistencyStatuses == null || !qualification.AccountConsistencyStatuses.Any())
			{
				state.AddConstructionError
					(
						String.Format("New BA Qualification {0}", qualificationKey),
						"New BAs collection is null or empty."
					);
			}
		}
	}
}
