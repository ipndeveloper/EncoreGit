using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common;
using NetSteps.Extensibility.Core;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Service
{
	public class PromotionOrderContextQualifier : IPromotionOrderContextQualifier
	{
		public PromotionQualificationResult GetQualificationResult(IPromotion promotion, IOrderContext orderContext)
		{
			var providerRegistry = Create.New<IDataObjectExtensionProviderRegistry>();
			var result = PromotionQualificationResult.MatchForAllCustomers;
			foreach (var qualification in promotion.PromotionQualifications.Values)
            {
                if (qualification == null)
                {
					throw new Exception("Promotion qualification was not loaded and therefore failed to match any order context.");
                }
                IPromotionQualificationHandler handler = (IPromotionQualificationHandler)providerRegistry.RetrieveExtensionProvider(qualification.ExtensionProviderKey);
                result.BitwiseAnd(handler.Matches(promotion, qualification, orderContext));
            }            
			return result;
		}
	}
}
