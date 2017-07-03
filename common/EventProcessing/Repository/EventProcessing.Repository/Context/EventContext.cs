using System.Data.Entity;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Repository.Entities;
using NetSteps.EventProcessing.Repository.Entity;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;

namespace NetSteps.EventProcessing.Repository.Context
{
	[ContainerRegister(typeof(IEventContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class EventContext : DbContext, IEventContext
	{
		public EventContext() : base("name=" + ConnectionStringNames.Core) { }

		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return base.Set<TEntity>();
		}

		public IDbSet<EventTypeEntity> EventTypes { get; set; }
		public IDbSet<EventEntity> Events { get; set; }
		public IDbSet<RecurringEventEntity> RecurringEvents { get; set; } 
	}

	public interface IEventContext : IDbContext
	{
		IDbSet<EventEntity> Events { get; }
		IDbSet<EventTypeEntity> EventTypes { get; }
		IDbSet<RecurringEventEntity> RecurringEvents { get; }
	}
}
