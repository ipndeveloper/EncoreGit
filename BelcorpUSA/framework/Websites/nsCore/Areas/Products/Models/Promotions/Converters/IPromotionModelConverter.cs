using System;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.Model;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	[ContractClass(typeof(PromotionModelConverterContract<,>))]
	public interface IPromotionModelConverter<TModel, TPromotion>
		where TPromotion : IPromotion
	{
		TModel Convert(TPromotion promotion);
		TPromotion Convert(TModel model);
	}

	[ContractClassFor(typeof(IPromotionModelConverter<,>))]
	public abstract class PromotionModelConverterContract<TModel, TPromotion> : IPromotionModelConverter<TModel, TPromotion>
 		where TPromotion : IPromotion
	{

		public TModel Convert(TPromotion promotion)
		{
			Contract.Requires<ArgumentNullException>(promotion != null);
			Contract.Ensures(Contract.Result<TModel>() != null);
			throw new NotImplementedException();
		}

		public TPromotion Convert(TModel model)
		{
			Contract.Requires<ArgumentNullException>(model != null);
			Contract.Ensures(Contract.Result<TPromotion>() != null);
			throw new NotImplementedException();
		}
	}
}