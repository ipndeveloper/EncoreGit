using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class AccountListQualificationHandler : BasePromotionQualificationHandler<IAccountListQualificationExtension, IAccountListQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IAccountHasTitleQualificationHandler
    {
        public AccountListQualificationHandler()
        {
        }
        
        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.AccountListProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            
            IAccountListQualificationExtension qualification = (IAccountListQualificationExtension)promotionQualification;
            var customerAccountIDs = orderContext.Order.OrderCustomers.Select(x => x.AccountID);
            var qualifiedAccounts = qualification.AccountNumbers.Intersect(customerAccountIDs);
            if (qualifiedAccounts.Count() > 0)
                return PromotionQualificationResult.MatchForSelectCustomers(qualifiedAccounts);
            return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IAccountListQualificationExtension);
            Contract.Assert(promotionQualification2 is IAccountListQualificationExtension);

            var extension1 = promotionQualification1 as IAccountListQualificationExtension;
            var extension2 = promotionQualification2 as IAccountListQualificationExtension;


            return extension1.AccountNumbers.Equals(extension2.AccountNumbers);
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, IAccountListQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.AccountNumbers == null || !qualification.AccountNumbers.Any())
			{
				state.AddConstructionError
					(
						string.Format("Promotion Qualification {0}", qualificationKey),
						"Account numbers is null or empty."
					);
			}
		}
	}
}
