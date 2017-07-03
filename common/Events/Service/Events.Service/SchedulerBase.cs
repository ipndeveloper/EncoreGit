using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.Events.Service
{
	public abstract class SchedulerBase
	{
		private IEventRepository _eventRepository;
		protected IEventRepository EventRepository
		{
			get
			{
				if (_eventRepository == null)
				{
					_eventRepository = Create.New<IEventRepository>();
				}
				return _eventRepository;
			}
		}
		private IEventTypeRepository _eventTypeRepository;
		protected IEventTypeRepository TypeRepository
		{
			get
			{
				if (_eventTypeRepository == null)
				{
					_eventTypeRepository = Create.New<IEventTypeRepository>();
				}
				return _eventTypeRepository;
			}
		}

		protected int GetEventTypeIDFromTermName()
		{
			string termName = GetEventTypeTermName();
			if (string.IsNullOrEmpty(termName))
			{
				throw new Exception("EventType TermName cannot be empty.");
			}
			IEventType eventTypeToSend = TypeRepository.GetByTermName(termName);
			if (eventTypeToSend == null)
			{
				throw new Exception(string.Format("There are no EventTypes with the TermName: {0}", termName));
			}
			return eventTypeToSend.EventTypeID;
		}

		protected abstract string GetEventTypeTermName();

		public int SaveEventAndGetEventID(DateTime timeToSend)
		{
			var toSave = Create.New<IEvent>();
			toSave.EventTypeID = GetEventTypeIDFromTermName();
			toSave.RetryCount = 0;
			toSave.ScheduledDateUTC = timeToSend;

			toSave = EventRepository.Save(toSave);
			return toSave.EventID;
		}
	}
}
