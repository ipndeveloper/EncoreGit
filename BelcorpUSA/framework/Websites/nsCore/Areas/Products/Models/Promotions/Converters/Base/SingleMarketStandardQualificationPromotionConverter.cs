using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.Converters.Base
{
	public class SingleMarketStandardQualificationPromotionConverter<TPromotion, TModel> : StandardQualificationPromotionConversion<TPromotion, TModel>
		where TPromotion : ISingleMarketStandardQualificationPromotion
		where TModel : ISingleMarketStandardQualificationPromotionModel
	{

		public override TModel Convert(TPromotion promotion)
		{
			var model = base.Convert(promotion);
			model.MarketID = promotion.MarketID;
			return model;
		}

		public override TPromotion Convert(TModel model)
		{
			var promotion = base.Convert(model);
			promotion.MarketID = model.MarketID;
			return promotion;
		}
	}
}