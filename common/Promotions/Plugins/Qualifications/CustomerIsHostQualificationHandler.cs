using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class CustomerIsHostQualificationHandler : BasePromotionQualificationHandler<ICustomerIsHostQualificationExtension, ICustomerIsHostQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, ICustomerIsHostQualificationHandler
    {
        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is ICustomerIsHostQualificationExtension);
            Contract.Assert(promotionQualification2 is ICustomerIsHostQualificationExtension);

            return true;
        }

        public override void CheckValidity(string qualificationKey, ICustomerIsHostQualificationExtension qualification, IPromotionState state)
        {
            // technically there is no extension, so no validity
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.CustomerIsHostProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is ICustomerIsHostQualificationExtension);

            var qualification = (ICustomerIsHostQualificationExtension)promotionQualification;
            var hostOrderCustomer = orderContext.Order.OrderCustomers.SingleOrDefault(x => x.OrderCustomerTypeID == 2);
            if (hostOrderCustomer != null)
                return PromotionQualificationResult.MatchForSelectCustomers(hostOrderCustomer.AccountID);
            return PromotionQualificationResult.NoMatch;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }
    }
}
