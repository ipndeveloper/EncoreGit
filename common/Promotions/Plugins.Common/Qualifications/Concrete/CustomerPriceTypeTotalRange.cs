using System;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class CustomerPriceTypeTotalRange : ICustomerPriceTypeTotalRange
	{
		public CustomerPriceTypeTotalRange()
		{
		}

		public CustomerPriceTypeTotalRange(decimal? minimum, decimal? maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		#region Implementation of ICustomerPriceTypeTotalRangeQualification

		public decimal? Minimum { get; set; }

		public decimal? Maximum { get; set; }

		#endregion
	}
}
