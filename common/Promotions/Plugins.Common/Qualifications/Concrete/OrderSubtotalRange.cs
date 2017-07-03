using System;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class OrderSubtotalRange : IOrderSubtotalRange
	{
		public OrderSubtotalRange()
		{
		}

		public OrderSubtotalRange(decimal? minimum, decimal? maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		#region Implementation of IOrderSubtotalRangeQualification

		public decimal? Minimum { get; set; }

		public decimal? Maximum { get; set; }

		#endregion
	}
}
