using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Components.Concrete
{
	[Serializable]
	public class ProductAdjustment : IProductAdjustment
	{
		public int MarketID { get; set; }

		public int PriceTypeID { get; set; }

		public decimal AdjustmentOperand { get; set; }
	}
}
