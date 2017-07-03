using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common
{
	using System;
	using System.Diagnostics.Contracts;

	internal abstract class IEventSchedulerContracts : IEventScheduler
	{
		public int Schedule(DateTime scheduledDateUtc, object parameters = null)
		{
			return 0;
		}

		public int SaveEventAndGetEventID(IEvent toSave)
		{
			throw new NotImplementedException();
		}

		public void Reschedule(int eventID)
		{
			Contract.Requires(eventID > 0);
		}

		public void Complete(int eventID)
		{
			Contract.Requires(eventID > 0);
		}
	}
}
