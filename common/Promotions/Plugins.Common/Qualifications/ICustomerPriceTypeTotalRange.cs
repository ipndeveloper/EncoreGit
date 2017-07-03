
namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
	public interface ICustomerPriceTypeTotalRange
	{
		decimal? Minimum { get; set; }
		decimal? Maximum { get; set; }
	}

	public static class ICustomerPriceTypeTotalRangeQualificationExtensions
	{
		public static bool IsEqualTo(this ICustomerPriceTypeTotalRange PriceTypeTotalQualification1, ICustomerPriceTypeTotalRange PriceTypeTotalQualification2)
		{
			return (PriceTypeTotalQualification1 == default(ICustomerPriceTypeTotalRange) && PriceTypeTotalQualification2 == default(ICustomerPriceTypeTotalRange))
				|| (PriceTypeTotalQualification1 != default(ICustomerPriceTypeTotalRange) && PriceTypeTotalQualification2 != default(ICustomerPriceTypeTotalRange)
					&& PriceTypeTotalQualification1.Minimum == PriceTypeTotalQualification2.Minimum
					&& PriceTypeTotalQualification1.Maximum == PriceTypeTotalQualification2.Maximum);
		}

		public static bool Overlaps(this ICustomerPriceTypeTotalRange PriceTypeTotalQualification1, ICustomerPriceTypeTotalRange PriceTypeTotalQualification2)
		{
			bool overlaps = false;
            if (PriceTypeTotalQualification2.Minimum.HasValue && PriceTypeTotalQualification1.IsInRange(PriceTypeTotalQualification2.Minimum.Value))
			{
				overlaps = true;
			}
			if (PriceTypeTotalQualification2.Maximum.HasValue && PriceTypeTotalQualification1.IsInRange(PriceTypeTotalQualification2.Maximum.Value))
			{
				overlaps = true;
			}
            if (PriceTypeTotalQualification1.Minimum.HasValue && PriceTypeTotalQualification2.IsInRange(PriceTypeTotalQualification1.Minimum.Value))
			{
				overlaps = true;
			}
			if (PriceTypeTotalQualification1.Maximum.HasValue && PriceTypeTotalQualification2.IsInRange(PriceTypeTotalQualification1.Maximum.Value))
			{
				overlaps = true;
			}
			return overlaps;
		}

		public static bool IsInRange(this ICustomerPriceTypeTotalRange PriceTypeTotalQualification, decimal queryValue)
		{
            return ((!PriceTypeTotalQualification.Minimum.HasValue || PriceTypeTotalQualification.Minimum.Value <= queryValue) && (!PriceTypeTotalQualification.Maximum.HasValue || PriceTypeTotalQualification.Maximum.Value >= queryValue));
		}
	}
}
