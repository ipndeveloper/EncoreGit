using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common;
using NetSteps.Events.PartyArguments.Common;

namespace NetSteps.Events.Service.PartyEvents
{
	public abstract class BasePartyEvent : SchedulerBase, IEventHandler
	{
		private IPartyEventArgumentRepository _argumentRepository;
		protected IPartyEventArgumentRepository ArgumentRepository
		{
			get
			{
				if (_argumentRepository == null)
				{
					_argumentRepository = Create.New<IPartyEventArgumentRepository>();
				}
				return _argumentRepository;
			}
		}

		public int ScheduleEventAndSaveArgs(int partyID)
		{
			Contract.Requires<ArgumentException>(partyID > 0);

			DateTime timeToSend = GetDateTimeForSchedule(partyID);
			int newEventID = SaveEventAndGetEventID(timeToSend);
			if (newEventID == 0)
			{
				throw new Exception("Failed to save new event");
			}
			var partyEventArg = Create.New<IPartyEventArgument>();
			partyEventArg.EventID = newEventID;
			partyEventArg.PartyID = partyID;

			ArgumentRepository.Save(partyEventArg);

			return newEventID;
		}

		protected abstract DateTime GetDateTimeForSchedule(int partyID);
		public abstract bool Execute(int eventID);
	}
}
