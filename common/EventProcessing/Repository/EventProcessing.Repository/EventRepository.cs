using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;
using NetSteps.EventProcessing.Repository.Context;
using NetSteps.EventProcessing.Repository.Entity;

namespace NetSteps.EventProcessing.Repository
{
	[ContainerRegister(typeof(IEventRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EventRepository : NetSteps.Foundation.Entity.EntityModelRepository<EventEntity, IEvent, IEventContext>, IEventRepository
	{
		public IEvent GetByEventID(int eventID)
		{
			using (var context = Create.New<IEventContext>())
			{
				return FirstOrDefault(context, eventID);
			}
		}

		public IEnumerable<IEvent> GetTopEvents(int top)
		{
			using (var context = Create.New<IEventContext>())
			{

				var joinedEvents = from e in context.Events
				       join t in context.EventTypes on e.EventTypeID equals t.EventTypeID
				       select new { Event = e, RetryCount = t.MaxRetryCount, t.Enabled };

				DateTime utcNow = DateTime.UtcNow;

				var eventEntities = joinedEvents
					.Where(je => je.Enabled
								 && je.Event.CompletedDateUTC == null
					             && je.Event.ScheduledDateUTC < utcNow
					             && (je.RetryCount == 0 || je.Event.RetryCount < je.RetryCount))
					.OrderBy(je => je.Event.ScheduledDateUTC)
					.Take(top).ToList();
				return eventEntities.Select(je => CreateModel(je.Event));
			}
		}


		public IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs)
		{
			using (var context = Create.New<IEventContext>())
			{
				return Where(context, x => eventIDs.Contains(x.EventID));
			}
		}

		public IEnumerable<IEvent> GetByEventTypeID(int eventTypeID)
		{
			using (var context = Create.New<IEventContext>())
			{
				return Where(context, x => x.EventTypeID == eventTypeID);
			}
		}

		public int GetEventTypeID(int eventID)
		{
			using (var context = Create.New<IEventContext>())
			{
				var entity = FirstOrDefault(context, x => x.EventID == eventID);
				return entity != null ? entity.EventTypeID : 0;
			}
		}

		public IEvent Save(IEvent Event)
		{
			using (var context = Create.New<IEventContext>())
			{
				bool exists = Any(context, c => c.EventID == Event.EventID);
				EventEntity savedEvent = exists ? Update(context, Event) : Add(context, Event);

				context.SaveChanges();
				UpdateModel(Event, savedEvent);

				return Event;
			}
		}

		public override Expression<Func<EventEntity, bool>> GetPredicateForModel(IEvent model)
		{
			return m => m.EventID == model.EventID;
		}

		public override void UpdateEntity(EventEntity entity, IEvent model)
		{
			entity.EventTypeID = model.EventTypeID;
			entity.ScheduledDateUTC = model.ScheduledDateUTC;
			entity.CompletedDateUTC = model.CompletedDateUTC;
			entity.RetryCount = model.RetryCount;
		}

		public override void UpdateModel(IEvent model, EventEntity entity)
		{
			model.EventID = entity.EventID;
			model.EventTypeID = entity.EventTypeID;
			model.ScheduledDateUTC = entity.ScheduledDateUTC;
			model.CompletedDateUTC = entity.CompletedDateUTC;
			model.RetryCount = entity.RetryCount;
		}
	}
}
