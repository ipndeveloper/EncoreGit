using System.Collections.Generic;
using System.Linq;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.Converters.Base
{
	public class MultipleMarketStandardQualificationPromotionConverter<TPromotion, TModel> : StandardQualificationPromotionConversion<TPromotion, TModel>
		where TPromotion : IMultipleMarketStandardQualificationPromotion
		where TModel : IMultipleMarketStandardQualificationPromotionModel
	{
		public override TModel Convert(TPromotion promotion)
		{
			var model = base.Convert(promotion);
			SetMarketIDs(model, promotion.MarketIDs);
			return model;
		}

		public void SetMarketIDs(TModel model, IEnumerable<int> marketIDs)
		{
			marketIDs.ToList().ForEach(x => {
												if (!model.MarketIDs.Contains(x))
												{
													model.MarketIDs.Add(x);
												}
											});
			model.HasMarketIDs = marketIDs.Count() > 0;
		}

		public override TPromotion Convert(TModel model)
		{
			var promotion = base.Convert(model);
			SetMarketIDs(promotion, model.HasMarketIDs, model.MarketIDs);
			return promotion;
		}


		public void SetMarketIDs(TPromotion promotion, bool hasMarketIDs, IList<int> marketIDs)
		{
			if (hasMarketIDs)
			{
				var added = marketIDs.Where(newmarket => !(promotion.MarketIDs.Contains(newmarket)));
				var removed = promotion.MarketIDs.Where(oldmarket => !marketIDs.Contains(oldmarket)).ToList();
				foreach (var newMarket in added)
				{
					promotion.AddMarketID(newMarket);
				}
				foreach (var oldMarket in removed)
				{
					promotion.DeleteMarketID(oldMarket);
				}
			}
			else
			{
				foreach (var existing in promotion.MarketIDs)
				{
					promotion.DeleteMarketID(existing);
				}
			}
		}
	}
}