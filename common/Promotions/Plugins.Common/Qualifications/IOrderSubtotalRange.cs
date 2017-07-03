
namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
	public interface IOrderPriceTypeTotalRange
	{
		decimal? Minimum { get; set; }
		decimal? Maximum { get; set; }
	}

	public static class IOrderPriceTypeTotalRangeQualificationExtensions
	{
		public static bool IsEqualTo(this IOrderPriceTypeTotalRange PriceTypeTotalQualification1, IOrderPriceTypeTotalRange PriceTypeTotalQualification2)
		{
			return (PriceTypeTotalQualification1 == default(IOrderPriceTypeTotalRange) && PriceTypeTotalQualification2 == default(IOrderPriceTypeTotalRange))
				|| (PriceTypeTotalQualification1 != default(IOrderPriceTypeTotalRange) && PriceTypeTotalQualification2 != default(IOrderPriceTypeTotalRange)
					&& PriceTypeTotalQualification1.Minimum == PriceTypeTotalQualification2.Minimum
					&& PriceTypeTotalQualification1.Maximum == PriceTypeTotalQualification2.Maximum);
		}

		public static bool Overlaps(this IOrderPriceTypeTotalRange PriceTypeTotalQualification1, IOrderPriceTypeTotalRange PriceTypeTotalQualification2)
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

		public static bool IsInRange(this IOrderPriceTypeTotalRange PriceTypeTotalQualification, decimal queryValue)
		{
			return ((!PriceTypeTotalQualification.Minimum.HasValue || PriceTypeTotalQualification.Minimum.Value <= queryValue) && (!PriceTypeTotalQualification.Maximum.HasValue || PriceTypeTotalQualification.Maximum.Value >= queryValue));
		}
	}
}
