using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common.Data
{
	[ContractClassFor(typeof(IEventTypeRepository))]
	internal abstract class IEventTypeRepositoryContracts : IEventTypeRepository
	{
		public IEventType GetByEventTypeID(int eventTypeID)
		{
			Contract.Requires(eventTypeID > 0);

			IEventType result = Contract.Result<IEventType>();
			Contract.Ensures(result == null || result.EventTypeID == eventTypeID);
			return default(IEventType);
		}

		public IEventType Save(IEventType eventType)
		{
			Contract.Requires(eventType != null);
			Contract.Requires(eventType.Name != null);
			Contract.Requires(eventType.TermName != null);
			Contract.Requires(eventType.EventHandler != null);
			Contract.Ensures(Contract.Result<IEventType>() != null);
			return default(IEventType);
		}

		public IEventType GetByTermName(string termName)
		{
			Contract.Requires(!string.IsNullOrEmpty(termName));
			return default(IEventType);
		}

		public List<IEventType> GetAllEventTypes()
		{
			Contract.Ensures(Contract.Result<List<IEventType>>() != null);
			return default(List<IEventType>);
		}

		public List<Tuple<int, int>> GetAllRetryCounts()
		{
			Contract.Ensures(Contract.Result<List<Tuple<int, int>>>() != null);
			return default(List<Tuple<int, int>>);
		}
	}
}
