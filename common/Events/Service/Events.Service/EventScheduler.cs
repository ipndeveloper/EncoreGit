using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Repositories;

namespace NetSteps.Events.Service
{
	[ContainerRegister(typeof(NetSteps.Events.Common.Services.IEventScheduler), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EventScheduler : NetSteps.Events.Common.Services.IEventScheduler
	{
		private IPartyRepository _partyRepository;
		protected IPartyRepository PartyRepository
		{
			get
			{
				if (_partyRepository == null)
				{
					_partyRepository = Create.New<IPartyRepository>();
				}
				return _partyRepository;
			}
		}

		private IOrderRepository _orderRepository;
		protected IOrderRepository OrderRepository
		{
			get
			{
				if (_orderRepository == null)
				{
					_orderRepository = Create.New<IOrderRepository>();
				}
				return _orderRepository;
			}
		}

		public virtual void SchedulePostEnrollmentEvents(int accountID)
		{

		}

		public virtual void SchedulePartyCompletionEvents(int partyID)
		{

		}

		public virtual void ScheduleOrderCompletionEvents(int orderID)
		{
		
		}
	}
}
