using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.EventProcessing.Common.Models;

namespace NetSteps.EventProcessing.Common.Data
{
	[ContractClass(typeof(IRecurringEventContracts))]
	public interface IRecurringEventRepository
	{
		/// <summary>
		/// Returns all active recurring events that are past their interval
		/// </summary>
		/// <returns></returns>
		IEnumerable<IRecurringEvent> GetActiveRecurringEvents();

		/// <summary>
		/// Returns the top X active recurring events that are past their interval
		/// </summary>
		/// <param name="top"></param>
		/// <returns></returns>
		IEnumerable<IRecurringEvent> GetActiveRecurringEvents(int top);

		/// <summary>
		/// Marks the given event's status to completed, and will show up in GetActiveRecurringEvents once the interval has past again
		/// </summary>
		/// <param name="recurringEvent"></param>
		void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent);

		/// <summary>
		/// Same as overload, but marks it done at a specific time
		/// </summary>
		/// <param name="recurringEvent"></param>
		/// <param name="dateTime"></param>
		void MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent, DateTime dateTime);
	}
	
	[ContractClassFor(typeof(IRecurringEventRepository))]
	internal abstract class IRecurringEventContracts : IRecurringEventRepository
	{
		IEnumerable<IRecurringEvent> IRecurringEventRepository.GetActiveRecurringEvents()
		{
			throw new NotImplementedException();
		}

		IEnumerable<IRecurringEvent> IRecurringEventRepository.GetActiveRecurringEvents(int top)
		{
			Contract.Requires<ArgumentException>(top > 0);
			throw new NotImplementedException();
		}

		void IRecurringEventRepository.MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent)
		{
			Contract.Requires<ArgumentNullException>(recurringEvent != null);
			Contract.Requires<ArgumentException>(recurringEvent.RecurringEventID > 0);
			throw new NotImplementedException();
		}

		void IRecurringEventRepository.MarkRecurringEventAsCompleted(IRecurringEvent recurringEvent, DateTime dateTime)
		{
			Contract.Requires<ArgumentNullException>(recurringEvent != null);
			Contract.Requires<ArgumentException>(recurringEvent.RecurringEventID > 0);
			throw new NotImplementedException();
		}
	}
}
