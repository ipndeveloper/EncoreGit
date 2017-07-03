using System;
using System.Linq;

namespace NetSteps.Data.Entities.Processors.Autoships
{
	using System.Diagnostics.Contracts;

	using NetSteps.Data.Entities.Cache;
	using NetSteps.Encore.Core.IoC;

	[ContainerRegister(typeof(IProcessorActivationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class ProcessorActivationRepository : IProcessorActivationRepository
	{
		public void ActivateSite(int siteID)
		{
			var site = Site.Load(siteID);
			site.SiteStatusID = (short)Constants.SiteStatus.Active;
			site.Save();
		}

		public void DeactivateSite(int siteID)
		{
			var site = Site.Load(siteID);
			site.SiteStatusID = (short)Constants.SiteStatus.DisabledForNonPayment;
			site.Save();
		}

		public IAutoshipScheduleDTO GetActiveSchedule(int autoshipScheduleID)
		{
			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.FirstOrDefault(a => a.Active && a.AutoshipScheduleID == autoshipScheduleID);

			if (autoshipSchedule == null)
			{
				return null;
			}

			var returnValue = Create.New<IAutoshipScheduleDTO>();
			returnValue.AutoshipScheduleTypeID = autoshipSchedule.AutoshipScheduleTypeID;
			returnValue.IntervalTypeID = autoshipSchedule.IntervalTypeID;
			return returnValue;
		}

		public ISiteDTO LoadSite(int accountID, int autoshipOrderID)
		{
			var site = Site.LoadByAccountID(accountID).FirstOrDefault(a => a.AutoshipOrderID == autoshipOrderID);

			if (site == null)
			{
				return null;
			}

			var returnValue = Create.New<ISiteDTO>();
			returnValue.SiteStatusID = site.SiteStatusID;
			returnValue.SiteID = site.SiteID;


			return returnValue;
		}

		public IIntervalTypeDTO LoadInterval(int intervalTypeID, DateTime runDate)
		{
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(intervalTypeID);
			if (intervalType == null)
			{
				return null;
			}

			var returnValue = Create.New<IIntervalTypeDTO>();
			returnValue.IntervalTypeID = intervalType.IntervalTypeID;
			returnValue.StartOfInterval = intervalType.GetStartOfInterval(runDate);
			returnValue.StartOfNextInterval = intervalType.GetStartOfNextInterval(runDate);

			return returnValue;
		}

		public int GetNumberOfIntervalsUnpaid(int intervalTypeID, DateTime nextRunDate, DateTime origin)
		{
			var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(intervalTypeID);

			// Start of next interval
			DateTime target = intervalType.GetStartOfInterval(nextRunDate);

			// Start of last paid interval
			DateTime current = intervalType.GetStartOfInterval(origin);
			int intervalCount = 1;

			DateTime startOfNextInterval = intervalType.GetStartOfNextInterval(current);
			while (startOfNextInterval != target)
			{
				current = startOfNextInterval;
				startOfNextInterval = intervalType.GetStartOfNextInterval(current);
				intervalCount++;
			}

			return intervalCount;
		}
	}
}
