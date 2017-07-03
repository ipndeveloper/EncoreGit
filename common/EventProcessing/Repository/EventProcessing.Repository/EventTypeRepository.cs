using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;
using NetSteps.EventProcessing.Repository.Context;
using NetSteps.EventProcessing.Repository.Entities;


namespace NetSteps.EventProcessing.Repository
{
	[ContainerRegister(typeof(IEventTypeRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EventTypeRepository : NetSteps.Foundation.Entity.EntityModelRepository<EventTypeEntity, IEventType, IEventContext>, IEventTypeRepository
	{
		public IEventType GetByEventTypeID(int eventTypeID)
		{
			using (var context = Create.New<IEventContext>())
			{
				return FirstOrDefault(context, x => x.EventTypeID == eventTypeID);
			}
		}

		public IEventType Save(IEventType eventType)
		{
			using (var context = Create.New<IEventContext>())
			{
				bool exists = Any(context, m => m.EventTypeID == eventType.EventTypeID);
				EventTypeEntity savedEntity = exists ? Update(context, eventType) : Add(context, eventType);

				context.SaveChanges();
				UpdateModel(eventType, savedEntity);

				return eventType;
			}
		}

		public IEventType GetByTermName(string termName)
		{
			using (var context = Create.New<IEventContext>())
			{
				return FirstOrDefault(context, et => et.TermName == termName);
			}
		}

		public List<IEventType> GetAllEventTypes()
		{
			using(var context = Create.New<IEventContext>())
			{
				return Where(context, c => true).ToList();
			}
		}

		public List<Tuple<int, int>> GetAllRetryCounts()
		{
			using(var context = Create.New<IEventContext>())
			{
				return Where(context, c => true)
					.Select(c => new Tuple<int, int>(c.EventTypeID, c.MaxRetryCount ?? 0))
					.ToList();
			}
		}

		public override Expression<Func<EventTypeEntity, bool>> GetPredicateForModel(IEventType model)
		{
			return m => m.EventTypeID == model.EventTypeID;
		}

		public override void UpdateEntity(EventTypeEntity entity, IEventType model)
		{
			entity.Description = model.Description;
			entity.Enabled = model.Enabled;
			entity.EventHandler = model.EventHandler;
			entity.MaxRetryCount = model.MaxRetryCount;
			entity.Name = model.Name;
			entity.TermName = model.TermName;
			entity.RetryInterval = model.RetryInterval.Seconds;
		}

		public override void UpdateModel(IEventType model, EventTypeEntity entity)
		{
			model.EventTypeID = entity.EventTypeID;
			model.Description = entity.Description;
			model.Enabled = entity.Enabled;
			model.EventHandler = entity.EventHandler;
			model.MaxRetryCount = entity.MaxRetryCount;
			model.Name = entity.Name;
			model.TermName = entity.TermName;
			model.RetryInterval = TimeSpan.FromSeconds(entity.RetryInterval);
		}
	}
}
