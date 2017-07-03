using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common.Data
{
	
	[ContractClass(typeof(IEventTypeRepositoryContracts))]
	public interface IEventTypeRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventTypeID"></param>
		/// <returns></returns>
		IEventType GetByEventTypeID(int eventTypeID);

		/// <summary>
		/// Saves an EventType record to the database.
		/// </summary>
		/// <param name="Event">The Event business object to save to the database</param>
		/// <returns>The Event business object</returns>
		IEventType Save(IEventType eventType);
		IEventType GetByTermName(string termName);
		List<IEventType> GetAllEventTypes();
		List<Tuple<int, int>> GetAllRetryCounts();
	}
}
