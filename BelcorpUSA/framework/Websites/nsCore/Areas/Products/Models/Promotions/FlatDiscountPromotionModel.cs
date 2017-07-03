using System.Collections.Generic;
using System.Diagnostics.Contracts;
using nsCore.Areas.Products.Models.Promotions.Base;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public class FlatDiscountPromotionModel : PriceAdjustmentPromotionModel, ISingleMarketStandardQualificationPromotionModel
	{
		public int MarketID { get; set; }
        public bool OrCondition { get; set; } //Has a combination of items

        public bool AndConditionQVTotal { get; set; } //Has a defined QV Total
        public decimal QvMin { get; set; }
        public decimal QvMax { get; set; }

		public List<FlatDiscountPromotionProductModel> PromotionProducts { get; set; }

		public FlatDiscountPromotionModel()
		{
			PromotionProducts = new List<FlatDiscountPromotionProductModel>();
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(PromotionProducts != null);
		}
	}
}
