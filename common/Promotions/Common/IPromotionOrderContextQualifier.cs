using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common
{
	[ContractClass(typeof(PromotionOrderContextQualifierContract))]
	public interface IPromotionOrderContextQualifier
	{
		PromotionQualificationResult GetQualificationResult(IPromotion promotion, IOrderContext orderContext);
	}

	[ContractClassFor(typeof(IPromotionOrderContextQualifier))]
	public abstract class PromotionOrderContextQualifierContract : IPromotionOrderContextQualifier
	{
		public PromotionQualificationResult GetQualificationResult(IPromotion promotion, IOrderContext orderContext)
		{
			Contract.Requires<ArgumentNullException>(promotion != null);
			Contract.Requires<ArgumentNullException>(orderContext != null);
			Contract.Ensures(Contract.Result<PromotionQualificationResult>() != null);
 			throw new NotImplementedException();
		}
	}	
}
