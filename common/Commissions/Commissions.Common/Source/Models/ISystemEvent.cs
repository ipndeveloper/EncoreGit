using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// System event contract
	/// </summary>
	public interface ISystemEvent
	{
		/// <summary>
		/// Gets or sets the created date.
		/// </summary>
		/// <value>
		/// The created date.
		/// </value>
		 DateTime CreatedDate { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [completed].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [completed]; otherwise, <c>false</c>.
		 /// </value>
		 bool Completed { get; set; }

		 /// <summary>
		 /// Gets or sets the duration.
		 /// </summary>
		 /// <value>
		 /// The duration.
		 /// </value>
		 int Duration { get; set; }

		 /// <summary>
		 /// Gets or sets the end time.
		 /// </summary>
		 /// <value>
		 /// The end time.
		 /// </value>
		 DateTime EndTime { get; set; }

		 /// <summary>
		 /// Gets or sets the period identifier.
		 /// </summary>
		 /// <value>
		 /// The period identifier.
		 /// </value>
		 int PeriodId { get; set; }

		 /// <summary>
		 /// Gets or sets the start time.
		 /// </summary>
		 /// <value>
		 /// The start time.
		 /// </value>
		 DateTime StartTime { get; set; }

		 /// <summary>
		 /// Gets or sets the system event application identifier.
		 /// </summary>
		 /// <value>
		 /// The system event application identifier.
		 /// </value>
		 int SystemEventApplicationId { get; set; }

		 /// <summary>
		 /// Gets or sets the system event identifier.
		 /// </summary>
		 /// <value>
		 /// The system event identifier.
		 /// </value>
		 int SystemEventId { get; set; }
	}
}
