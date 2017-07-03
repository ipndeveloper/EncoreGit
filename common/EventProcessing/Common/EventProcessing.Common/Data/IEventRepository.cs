using NetSteps.EventProcessing.Common.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.EventProcessing.Common.Data
{
	[ContractClass(typeof(IEventRepositoryContracts))]
	public interface IEventRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventID"></param>
		/// <returns></returns>
		IEvent GetByEventID(int eventID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="top"></param>
		/// <returns></returns>
		IEnumerable<IEvent> GetTopEvents(int top);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventIDs"></param>
		/// <returns></returns>
		IEnumerable<IEvent> GetByEventIDs(IEnumerable<int> eventIDs);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventTypeID"></param>
		/// <returns></returns>
		IEnumerable<IEvent> GetByEventTypeID(int eventTypeID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventID"></param>
		/// <returns></returns>
		int GetEventTypeID(int eventID);

		/// <summary>
		/// Saves an Event record to the database.
		/// </summary>
		/// <param name="Event">The Event business object to save to the database</param>
		/// <returns>The Event business object</returns>
		IEvent Save(IEvent Event);
	}
}
