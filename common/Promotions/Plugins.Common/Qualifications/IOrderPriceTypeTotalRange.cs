﻿
namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
	public interface IOrderSubtotalRange
	{
		decimal? Minimum { get; set; }
		decimal? Maximum { get; set; }
	}

	public static class IOrderSubtotalRangeQualificationExtensions
	{
		public static bool IsEqualTo(this IOrderSubtotalRange subtotalQualification1, IOrderSubtotalRange subtotalQualification2)
		{
			return (subtotalQualification1 == default(IOrderSubtotalRange) && subtotalQualification2 == default(IOrderSubtotalRange))
				|| (subtotalQualification1 != default(IOrderSubtotalRange) && subtotalQualification2 != default(IOrderSubtotalRange)
					&& subtotalQualification1.Minimum == subtotalQualification2.Minimum
					&& subtotalQualification1.Maximum == subtotalQualification2.Maximum);
		}

		public static bool Overlaps(this IOrderSubtotalRange subtotalQualification1, IOrderSubtotalRange subtotalQualification2)
		{
			bool overlaps = false;
            if (subtotalQualification2.Minimum.HasValue && subtotalQualification1.IsInRange(subtotalQualification2.Minimum.Value))
			{
				overlaps = true;
			}
			if (subtotalQualification2.Maximum.HasValue && subtotalQualification1.IsInRange(subtotalQualification2.Maximum.Value))
			{
				overlaps = true;
			}
            if (subtotalQualification1.Minimum.HasValue && subtotalQualification2.IsInRange(subtotalQualification1.Minimum.Value))
			{
				overlaps = true;
			}
			if (subtotalQualification1.Maximum.HasValue && subtotalQualification2.IsInRange(subtotalQualification1.Maximum.Value))
			{
				overlaps = true;
			}
			return overlaps;
		}

		public static bool IsInRange(this IOrderSubtotalRange subtotalQualification, decimal queryValue)
		{
			return ((!subtotalQualification.Minimum.HasValue || subtotalQualification.Minimum.Value <= queryValue) && (!subtotalQualification.Maximum.HasValue || subtotalQualification.Maximum.Value >= queryValue));
		}
	}
}
