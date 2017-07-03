using System;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: TimeSpan Extensions
	/// Created: 01-30-2009
	/// </summary>
	public static class TimeSpanExtensions
	{
		public static bool IsEmpty(this TimeSpan timeSpan)
		{
			return timeSpan == TimeSpan.FromSeconds(0);
		}

		// TODO: also show the sub unit: 5 minutes 4 seconds
		public static string ToFriendlyString(this TimeSpan timeSpan, bool useUnitShorthand)
		{
			if (timeSpan.TotalSeconds < 1)
				return string.Format("{0} {1}", timeSpan.TotalMilliseconds.TruncateDoubleInsertCommas(2), (useUnitShorthand) ? "milliseconds" : "milliseconds");
			else if (timeSpan.TotalMinutes < 1)
				return string.Format("{0} {1}", timeSpan.TotalSeconds.TruncateDoubleInsertCommas(2), (useUnitShorthand) ? "seconds" : "sec");
			else if (timeSpan.TotalHours < 1)
				return string.Format("{0} {1}", timeSpan.TotalMinutes.TruncateDoubleInsertCommas(2), (useUnitShorthand) ? "minutes" : "min");
			else if (timeSpan.TotalDays < 1)
				return string.Format("{0} hours", timeSpan.TotalHours.TruncateDoubleInsertCommas(2));
			else
				return string.Format("{0} days", timeSpan.TotalDays.TruncateDoubleInsertCommas(2));
		}
	}
}
