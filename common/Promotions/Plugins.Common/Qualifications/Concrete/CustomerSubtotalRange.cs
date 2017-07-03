using System;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class CustomerSubtotalRange : ICustomerSubtotalRange
	{
		public CustomerSubtotalRange()
		{
		}

		public CustomerSubtotalRange(decimal? minimum, decimal? maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		#region Implementation of ICustomerSubtotalRangeQualification

		public decimal? Minimum { get; set; }

		public decimal? Maximum { get; set; }

		#endregion
	}
}
