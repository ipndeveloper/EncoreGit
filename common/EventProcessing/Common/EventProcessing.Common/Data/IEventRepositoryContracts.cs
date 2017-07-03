using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common.Data
{
	


	[ContractClassFor(typeof(IEventRepository))]
	internal abstract class IEventRepositoryContracts : IEventRepository
	{
		public IEvent GetByEventID(int eventID)
		{
			Contract.Requires(eventID > 0);
			IEvent result = Contract.Result<IEvent>();
			Contract.Ensures(result == null || result.EventID == eventID);
			return default(IEvent);
		}

		public IEnumerable<IEvent> GetTopEvents(int top)
		{
			Contract.Requires(top > 0);
			Contract.Ensures(Contract.Result<IEnumerable<IEvent>>() != null);
			return default(IEvent[]);
		}

		public IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs)
		{
			Contract.Requires(eventIDs.All(eid => eid > 0));
			Contract.Ensures(Contract.Result<IEnumerable<IEvent>>() != null);
			return default(List<IEvent>);
		}

		public IEnumerable<IEvent> GetByEventTypeID(int eventTypeID)
		{
			Contract.Requires(eventTypeID > 0);
			IEnumerable<IEvent> result = Contract.Result<IEnumerable<IEvent>>();
			Contract.Ensures(result != null && !result.Any(x => x.EventTypeID != eventTypeID));
			return default(List<IEvent>);
		}

		public int GetEventTypeID(int eventID)
		{
			Contract.Requires(eventID > 0);
			Contract.Ensures(Contract.Result<int>() > 0);
			return default(int);
		}

		public IEvent Save(IEvent Event)
		{
			Contract.Requires(Event.EventTypeID > 0);
			Contract.Ensures(Contract.Result<IEvent>() != null);
			return default(IEvent);
		}

		public void OpenContext()
		{
		}

		public void CloseContext()
		{
		}
	}
}
