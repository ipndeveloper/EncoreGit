using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class OrderHasMinimumProductSelectionsQualificationHandler : BasePromotionQualificationHandler<IOrderHasMinimumProductSelectionsQualificationExtension, IOrderHasMinimumProductSelectionsQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IOrderHasMinimumProductSelectionsQualificationHandler
    {
        public OrderHasMinimumProductSelectionsQualificationHandler()
        {
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            throw new NotImplementedException();
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.OrderHasMinimumProductOptionsProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            throw new NotImplementedException();
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            throw new NotImplementedException();
        }

		public override void CheckValidity(string qualificationKey, IOrderHasMinimumProductSelectionsQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.NumberOfOptionsRequired < 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Number of options required cannot be less than 0."
					);
			}
			if (qualification.ProductOptions == null || !qualification.ProductOptions.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Product Options collection is null or empty."
					);
			}
		}
	}
}
