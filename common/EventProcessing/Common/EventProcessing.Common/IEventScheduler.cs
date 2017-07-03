using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common
{
	public interface IEventScheduler
	{
		/// <summary>
		/// Schedule an event
		/// </summary>
		/// <param name="scheduledDateUtc">When the event is to be run</param>
		/// <param name="parameters">An object that contains the event parameters</param>
		/// <returns>The EventID of the newly-created event</returns>
		int SaveEventAndGetEventID(IEvent toSave);

		/// <summary>
		/// Reschedule an event when it fails to run
		/// </summary>
		/// <param name="eventID">The EventID of the event that needs to be rescheduled</param>
		void Reschedule(int eventID);
	}
}
