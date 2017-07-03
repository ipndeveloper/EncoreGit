using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission run step
	/// </summary>
	public interface ICommissionRunStep
	{
		/// <summary>
		/// Gets or sets the name of the commission procedure.
		/// </summary>
		/// <value>
		/// The name of the commission procedure.
		/// </value>
		 string CommissionProcedureName { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run identifier.
		 /// </summary>
		 /// <value>
		 /// The commission run identifier.
		 /// </value>
		 int CommissionRunId { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run procedure identifier.
		 /// </summary>
		 /// <value>
		 /// The commission run procedure identifier.
		 /// </value>
		 int CommissionRunProcedureId { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run step identifier.
		 /// </summary>
		 /// <value>
		 /// The commission run step identifier.
		 /// </value>
		 int CommissionRunStepId { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run step number.
		 /// </summary>
		 /// <value>
		 /// The commission run step number.
		 /// </value>
		 int CommissionRunStepNumber { get; set; }

		 /// <summary>
		 /// Gets or sets the display message.
		 /// </summary>
		 /// <value>
		 /// The display message.
		 /// </value>
		 string DisplayMessage { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [enabled].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
		 /// </value>
		 bool Enabled { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [include period identifier parameter].
		 /// </summary>
		 /// <value>
		 /// <c>true</c> if [include period identifier parameter]; otherwise, <c>false</c>.
		 /// </value>
		 bool IncludePeriodIdParameter { get; set; }

		 /// <summary>
		 /// Gets or sets the sort order.
		 /// </summary>
		 /// <value>
		 /// The sort order.
		 /// </value>
		 int SortOrder { get; set; }

		 /// <summary>
		 /// Gets or sets the SQL time out seconds.
		 /// </summary>
		 /// <value>
		 /// The SQL time out seconds.
		 /// </value>
		 int SQLTimeOutSeconds { get; set; }

		 /// <summary>
		 /// Gets or sets the system event log.
		 /// </summary>
		 /// <value>
		 /// The system event log.
		 /// </value>
		 ISystemEventLog SystemEventLog { get; set; }
	}
}
