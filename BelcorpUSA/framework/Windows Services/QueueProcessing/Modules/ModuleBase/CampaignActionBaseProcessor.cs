namespace NetSteps.QueueProcessing.Modules.ModuleBase
{


    public abstract class CampaignActionBaseProcessor<T> : QueueProcessor<T>
    {
        public CampaignActionBaseProcessor()
            : base()
        {
            this.Name = "CampaignActionBaseProcessor";
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            throw new System.NotImplementedException();
        }

        public override void ProcessQueueItem(T item)
        {
            throw new System.NotImplementedException();
        }

		//protected virtual bool RunActionNow(NetSteps.Data.Entities.CampaignAction action)
		//{
		//    bool runActionNow = false;
		//    DateTime? nextRunDate = null;

		//    if (action.RunImmediately)
		//        runActionNow = true;
		//    else if (action.Interval.HasValue && action.IntervalTimeUnitTypeID.HasValue)
		//    {
		//        // Calculate nextRunDate based on interval value and 'Base Date' - JHE

		//        // TODO: Find BaseDate based on CampaignType/EventType - JHE
		//        DateTime baseDate = DateTime.Now;

		//        //Added this following logic to make the service run on interval mode
		//        //If the basedate is datetime.now ,service will not behave with IntervalUnit mode ie set in CampaignActions table
		//        if (action.NextRunDateUTC != null)
		//            baseDate = (DateTime)action.NextRunDateUTC;

		//        if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Seconds.ToShort())
		//            nextRunDate = baseDate.AddSeconds(action.Interval.Value);
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Minutes.ToShort())
		//            nextRunDate = baseDate.AddMinutes(action.Interval.Value);
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Hours.ToShort())
		//            nextRunDate = baseDate.AddHours(action.Interval.Value);
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Days.ToShort())
		//            nextRunDate = baseDate.AddDays(action.Interval.Value);
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Weeks.ToShort())
		//        {
		//            int days = (action.Interval.Value * 7);
		//            nextRunDate = baseDate.AddDays(days);
		//        }
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Months.ToShort())
		//            nextRunDate = baseDate.AddMonths(action.Interval.Value);
		//        else if (action.IntervalTimeUnitTypeID == Constants.TimeUnitType.Years.ToShort())
		//            nextRunDate = baseDate.AddYears(action.Interval.Value);

		//        if (nextRunDate.HasValue && nextRunDate.Value <= DateTime.Now)
		//            runActionNow = true;
		//    }
		//    else if (action.NextRunDate.HasValue)
		//    {
		//        nextRunDate = action.NextRunDate.Value;

		//        if (nextRunDate.HasValue && nextRunDate.Value <= DateTime.Now)
		//            runActionNow = true;
		//    }
		//    return runActionNow;
		//}
    }
}
