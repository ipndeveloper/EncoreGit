using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common;
using NetSteps.Events.AccountArguments.Common;

namespace NetSteps.Events.Service.AccountEvents
{
	public abstract class BaseAccountEvent : SchedulerBase, IEventHandler
	{
		private IAccountEventArgumentRepository argRepository;
		protected IAccountEventArgumentRepository ArgumentRepository
		{
			get
			{
				if (argRepository == null) argRepository = Create.New<IAccountEventArgumentRepository>();
				return argRepository;
			}
		}

		public abstract bool Execute(int eventID);

		public int ScheduleEventAndSaveArgs(int accountID)
		{
			Contract.Requires<ArgumentException>(accountID > 0);

			DateTime timeToSend = GetDateTimeForSchedule(accountID);
			int newEventID = SaveEventAndGetEventID(timeToSend);
			if (newEventID < 1)
			{
				throw new Exception("Failed to save new event");
			}

			var accountEventArg = Create.New<IAccountEventArgument>();
			accountEventArg.EventID = newEventID;
			accountEventArg.AccountID = accountID;

			ArgumentRepository.Save(accountEventArg);

			return newEventID;
		}

		protected abstract DateTime GetDateTimeForSchedule(int accountID);
	}
}
