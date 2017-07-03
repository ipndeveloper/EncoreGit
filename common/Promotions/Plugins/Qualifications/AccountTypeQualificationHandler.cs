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
	public class AccountTypeQualificationHandler : BasePromotionQualificationHandler<IAccountTypeQualificationExtension, IAccountTypeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IAccountTypeQualificationHandler
    {
        public AccountTypeQualificationHandler()
        {
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IAccountTypeQualificationExtension);

            IAccountTypeQualificationExtension qualification = (IAccountTypeQualificationExtension)promotionQualification;
            var customerAccountTypeIDs = orderContext.Order.OrderCustomers.Select(x => x.AccountTypeID).Distinct();
            var qualifiedAccountTypes = qualification.AccountTypes.Intersect(customerAccountTypeIDs);
            var qualifiedCustomerIDs = orderContext.Order.OrderCustomers.Where(x => qualifiedAccountTypes.Contains(x.AccountTypeID)).Select(x => x.AccountID);
            if (qualifiedCustomerIDs.Count() > 0)
                return PromotionQualificationResult.MatchForSelectCustomers(qualifiedCustomerIDs);
            return PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IAccountTypeQualificationExtension);
            Contract.Assert(promotionQualification2 is IAccountTypeQualificationExtension);

            var extension1 = promotionQualification1 as IAccountTypeQualificationExtension;
            var extension2 = promotionQualification2 as IAccountTypeQualificationExtension;

            if (extension1.AccountTypes.Except(extension2.AccountTypes).Count() > 0)
                return false;
            if (extension2.AccountTypes.Except(extension1.AccountTypes).Count() > 0)
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
            return QualificationExtensionProviderKeys.AccountTypeProviderKey;
        }

		public override void CheckValidity(string qualificationKey, IAccountTypeQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.AccountTypes == null || !qualification.AccountTypes.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Account Types collection is null or empty."
					);
			}
		}
	}
}
