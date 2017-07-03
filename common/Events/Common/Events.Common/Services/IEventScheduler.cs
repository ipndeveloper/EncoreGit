namespace NetSteps.Events.Common.Services
{
	public interface IEventScheduler
	{
		void SchedulePostEnrollmentEvents(int accountID);
		void SchedulePartyCompletionEvents(int partyID);
		void ScheduleOrderCompletionEvents(int orderID);
	}
}
