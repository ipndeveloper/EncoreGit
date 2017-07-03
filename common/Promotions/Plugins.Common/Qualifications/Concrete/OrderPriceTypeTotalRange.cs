using System;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderPriceTypeTotalRange : IOrderPriceTypeTotalRange
	{
		public OrderPriceTypeTotalRange()
		{
		}

		public OrderPriceTypeTotalRange(decimal? minimum, decimal? maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		#region Implementation of IOrderPriceTypeTotalRangeQualification

		public decimal? Minimum { get; set; }

		public decimal? Maximum { get; set; }

		#endregion
	}
}
