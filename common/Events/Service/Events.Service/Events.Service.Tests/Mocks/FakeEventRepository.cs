using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common.Data;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeEventRepository : IEventRepository
	{
		public static IEvent eventToReturn;
		public static int eventTypeID;

		public static void Initialize()
		{
			var root = Container.Root;
			root.ForType<IEventRepository>()
				.Register<FakeEventRepository>()
				.ResolveAsSingleton()
				.End();

			eventToReturn = Create.New<IEvent>();
			eventToReturn.EventID = 1;
			eventToReturn.EventTypeID = 1;
			eventTypeID = 1;

		}

		public IEvent GetByEventID(int eventID)
		{
			return eventToReturn;
		}

		public IEnumerable<IEvent> GetTopEvents(int top)
		{
			return new[] { eventToReturn };
		}

		public IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs)
		{
			return new[] { eventToReturn };
		}

		public IEnumerable<IEvent> GetByEventTypeID(int eventTypeID)
		{
			return new IEvent[] { eventToReturn };
		}

		public int GetEventTypeID(int eventID)
		{
			return eventTypeID;
		}

		public IEvent Save(IEvent Event)
		{
			return eventToReturn;
		}
	}
}
