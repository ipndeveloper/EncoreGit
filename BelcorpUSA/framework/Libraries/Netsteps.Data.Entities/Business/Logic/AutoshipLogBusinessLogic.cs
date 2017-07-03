using System;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AutoshipLogBusinessLogic
	{
		public virtual int GetTotalAttemptsInInterval(
			IAutoshipLogRepository repository,
			DateTime date,
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(autoshipSchedule.IntervalTypeID);

			return GetTotalAttemptsInInterval(repository, date, autoshipOrder.TemplateOrderID, intervalType);
		}

		public virtual int GetTotalAttemptsInInterval(
			IAutoshipLogRepository repository,
			DateTime date,
			int templateOrderID,
			IntervalType intervalType)
		{
			if (intervalType == null)
			{
				throw new ArgumentNullException("intervalType");
			}

			var startOfInterval = intervalType.GetStartOfInterval(date);
			var startOfNextInterval = intervalType.GetStartOfNextInterval(date);

			var allLogEntryTimes =
				repository.Where(
					x => x.TemplateOrderID == templateOrderID && x.RunDate >= startOfInterval && x.RunDate < startOfNextInterval)
					.Select(re => re.DateCreatedUTC).OrderBy(re => re);

			int count = 0;
			DateTime? lastOne = null;
			foreach (DateTime currentDate in allLogEntryTimes)
			{
				if (!lastOne.HasValue)
				{
					lastOne = currentDate;
					count++;
					continue;
				}

				var difference = currentDate.Subtract(lastOne.Value);
				if (difference.TotalMinutes < 2)
				{
					continue;
				}
				count++;
				lastOne = currentDate;
			}

			return count;
		}

		public virtual int GetTotalAttemptsSinceLastSuccessful(
			IAutoshipLogRepository repository,
			DateTime lastSuccessfulDate,
			AutoshipOrder autoshipOrder)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			return GetTotalAttemptsSinceLastSuccessful(repository, lastSuccessfulDate, autoshipOrder.TemplateOrderID);
		}

		public virtual int GetTotalAttemptsSinceLastSuccessful(
			IAutoshipLogRepository repository,
			DateTime lastSuccessfulDate,
			int templateOrderID)
		{
			return repository.Count(x =>
				x.TemplateOrderID == templateOrderID
				&& x.RunDate > lastSuccessfulDate
			);
		}
	}
}
