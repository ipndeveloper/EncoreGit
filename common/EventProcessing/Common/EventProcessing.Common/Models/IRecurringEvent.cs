using System;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.EventProcessing.Common.Models
{
	[DTO]
	public interface IRecurringEvent
	{
		/// <summary>
		/// Primary Key
		/// </summary>
		int RecurringEventID { get; set; }

		/// <summary>
		/// Joins over to EventTypes, determines what class should be run for this event
		/// </summary>
		int EventTypeID { get; set; }

		/// <summary>
		/// Amount of time between runs
		/// </summary>
		int IntervalInMinutes { get; set; }

		/// <summary>
		/// Last time the event successfully ran
		/// </summary>
		DateTime? LastRunDateUTC { get; set; }

		/// <summary>
		/// Determines whether the event should be used
		/// </summary>
		bool IsActive { get; set; }
	}
}
