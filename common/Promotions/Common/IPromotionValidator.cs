using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common
{
	[ContractClass(typeof(PromotionValidatorContract))]
	public interface IPromotionValidator
	{
		IPromotionState CheckValidity(IPromotion promotion);
	}

	[ContractClassFor(typeof(IPromotionValidator))]
	public abstract class PromotionValidatorContract : IPromotionValidator
	{

		public IPromotionState CheckValidity(IPromotion promotion)
		{
			Contract.Requires<ArgumentNullException>(promotion != null);
			Contract.Ensures(Contract.Result<IPromotionState>() != null);
			throw new NotImplementedException();
		}
	}
}
