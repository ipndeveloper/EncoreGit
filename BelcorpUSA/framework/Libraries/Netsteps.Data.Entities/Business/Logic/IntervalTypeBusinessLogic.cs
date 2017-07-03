using System;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class IntervalTypeBusinessLogic
	{
		public virtual DateTime GetStartOfInterval(
			DateTime date,
			IntervalType intervalType)
		{
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}

			if (intervalType.IsWeekly)
			{
				return date.FirstDayOfWeek();
			}

			if (intervalType.IsAnnual)
			{
				return date.FirstDayOfYear();
			}

			// Not annual, not weekly, assume monthly
			return date.FirstDayOfMonth();

		}

		public virtual DateTime GetStartOfNextInterval(
			DateTime date,
			IntervalType intervalType)
		{
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}

			if (intervalType.IsWeekly)
			{
				return GetStartOfInterval(date.AddDays(intervalType.Interval * 7), intervalType);
			}

			if (intervalType.IsAnnual)
			{
				return GetStartOfInterval(date.AddYears(intervalType.Interval), intervalType);
			}

			// Not annual, not weekly, assume monthly
			return GetStartOfInterval(date.AddMonths(intervalType.Interval), intervalType);
		}
	}
}
