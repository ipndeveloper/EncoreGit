using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// System event log.  Not sure how this is different than System Event.
	/// </summary>
	public interface ISystemEventLog
	{
		/// <summary>
		/// Gets or sets the system event log identifier.
		/// </summary>
		/// <value>
		/// The system event log identifier.
		/// </value>
		 int SystemEventLogId { get; set; }

		/// <summary>
		/// Gets or sets the system event application identifier.
		/// </summary>
		/// <value>
		/// The system event application identifier.
		/// </value>
		 int SystemEventApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the system event log type identifier.
		/// </summary>
		/// <value>
		/// The system event log type identifier.
		/// </value>
		 int SystemEventLogTypeId { get; set; }

		/// <summary>
		/// Gets or sets the event message.
		/// </summary>
		/// <value>
		/// The event message.
		/// </value>
		 string EventMessage { get; set; }

		/// <summary>
		/// Gets or sets the created date.
		/// </summary>
		/// <value>
		/// The created date.
		/// </value>
		 DateTime CreatedDate { get; set; }
	}
}
