using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using nsCore.Areas.Products.Models.Promotions.Base;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public class PercentOffPromotionModel : PriceAdjustmentPromotionModel, IMultipleMarketStandardQualificationPromotionModel
	{
		public IList<int> MarketIDs { get; private set; }

		public List<PercentOffPromotionProductModel> PromotionProducts { get; set; }

		public PercentOffPromotionModel()
		{
            MarketIDs = new List<int>();
            PromotionProducts = new List<PercentOffPromotionProductModel>();
		}

		public PercentOffPromotionModel LoadResources(IProductPromotionPercentDiscount promotion)
		{
			//todo: implement
			return this;
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(PromotionProducts != null);
		}

		public bool HasMarketIDs { get; set; }
        public bool OrCondition { get; set; }//Has a combination of items

        public bool AndConditionQVTotal { get; set; } //Has a defined QV Total
        public decimal QvMin { get; set; }
        public decimal QvMax { get; set; }
	}
}