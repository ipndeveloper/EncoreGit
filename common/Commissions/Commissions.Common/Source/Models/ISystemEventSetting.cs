using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// System event settings
	/// </summary>
	public interface ISystemEventSetting
	{
		/// <summary>
		/// Gets or sets the prep system event application identifier.
		/// </summary>
		/// <value>
		/// The prep system event application identifier.
		/// </value>
		 int PrepSystemEventApplicationId { get; set; }

		 /// <summary>
		 /// Gets or sets the publish system event application identifier.
		 /// </summary>
		 /// <value>
		 /// The publish system event application identifier.
		 /// </value>
		 int PublishSystemEventApplicationId { get; set; }

		 /// <summary>
		 /// Gets or sets the system event log error type identifier.
		 /// </summary>
		 /// <value>
		 /// The system event log error type identifier.
		 /// </value>
		 int SystemEventLogErrorTypeId { get; set; }

		 /// <summary>
		 /// Gets or sets the system event log notice type identifier.
		 /// </summary>
		 /// <value>
		 /// The system event log notice type identifier.
		 /// </value>
		 int SystemEventLogNoticeTypeId { get; set; }
	}
}
