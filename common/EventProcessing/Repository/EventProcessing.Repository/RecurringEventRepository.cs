using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;
using NetSteps.EventProcessing.Repository.Context;
using NetSteps.EventProcessing.Repository.Entities;

namespace NetSteps.EventProcessing.Repository
{
	[ContainerRegister(typeof(IRecurringEventRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class RecurringEventRepository : NetSteps.Foundation.Entity.EntityModelRepository<RecurringEventEntity, IRecurringEvent, IEventContext>, IRecurringEventRepository
	{
		public override Expression<Func<RecurringEventEntity, bool>> GetPredicateForModel(IRecurringEvent model)
		{
			return m => m.RecurringEventID == model.RecurringEventID;
		}

		public override void UpdateEntity(RecurringEventEntity entity, IRecurringEvent model)
		{
			entity.EventTypeID = model.EventTypeID;
			entity.IntervalInMinutes = model.IntervalInMinutes;
			entity.IsActive = model.IsActive;
			entity.LastRunDateUTC = model.LastRunDateUTC;
		}

		public override void UpdateModel(IRecurringEvent model, RecurringEventEntity entity)
		{
			model.EventTypeID = entity.EventTypeID;
			model.IntervalInMinutes = entity.IntervalInMinutes;
			model.IsActive = entity.IsActive;
			model.LastRunDateUTC = entity.LastRunDateUTC;
			model.RecurringEventID = entity.RecurringEventID;
		}

		public IEnumerable<IRecurringEvent> GetActiveRecurringEvents()
		{
			using (var context = Create.New<IEventContext>())
			{
				DateTime now = DateTime.UtcNow;
				var activeEvents = context.RecurringEvents
									.Where(r => r.IsActive)
									.ToList();

				return activeEvents
					.Where(r => r.LastRunDateUTC == null || r.LastRunDateUTC.Value.AddMinutes(r.IntervalInMinutes) < now)
					.Select(createRecurringEventModel);
			}
		}

		public IEnumerable<IRecurringEvent> GetActiveRecurringEvents(int top)
		{
			using(var context = Create.New<IEventContext>())
			{
				DateTime now = DateTime.UtcNow;
				var activeEvents = context.RecurringEvents
									.Where(r => r.IsActive)
									.ToList();

				return activeEvents
							.Select(r => new { Event = r, PastDue = r.LastRunDateUTC == null ? 0 : (r.LastRunDateUTC.Value.AddMinutes(r.IntervalInMinutes) - now).TotalMinutes })
							.Where(r => r.PastDue <= 0)
							.OrderByDescending(r => r.PastDue)
							.Take(top)
							.Select(r => createRecurringEventModel(r.Event));
			}
		}

		public void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent)
		{
			MarkRecurringEventAsCompleted(recurringEvent, DateTime.UtcNow);
		}

		public void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent, DateTime dateTime)
		{
			using (var context = Create.New<IEventContext>())
			{
				recurringEvent.LastRunDateUTC = dateTime;

				var dbEntity = context.RecurringEvents.FirstOrDefault(m => m.RecurringEventID == recurringEvent.RecurringEventID);

				if (dbEntity == null)
				{
					throw new Exception(string.Format("There are no entities in the database for RecurringEventID {0}", recurringEvent.RecurringEventID));
				}

				UpdateEntity(dbEntity, recurringEvent);
				context.SaveChanges();
			}
		}

		private IRecurringEvent createRecurringEventModel(RecurringEventEntity entity)
		{
			var returnValue = Create.New<IRecurringEvent>();
			UpdateModel(returnValue, entity);
			return returnValue;
		}
	}
}
