using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission run
	/// </summary>
	public interface ICommissionRun : IJsonSerializable
	{
		/// <summary>
		/// Gets or sets the commission run identifier.
		/// </summary>
		/// <value>
		/// The commission run identifier.
		/// </value>
		 int CommissionRunId { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [commission run can be published].
		 /// </summary>
		 /// <value>
		 /// <c>true</c> if [commission run can be published]; otherwise, <c>false</c>.
		 /// </value>
		 bool CommissionRunCanBePublished { get; set; }

		 /// <summary>
		 /// Gets or sets the name of the commission run.
		 /// </summary>
		 /// <value>
		 /// The name of the commission run.
		 /// </value>
		 string CommissionRunName { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run plans.
		 /// </summary>
		 /// <value>
		 /// The commission run plans.
		 /// </value>
		 IEnumerable<ICommissionRunPlan> CommissionRunPlans { get; set; }

		 /// <summary>
		 /// Gets the commission run progress.
		 /// </summary>
		 /// <value>
		 /// The commission run progress.
		 /// </value>
		 int CommissionRunProgress { get; }

		 /// <summary>
		 /// Gets or sets the commission run steps.
		 /// </summary>
		 /// <value>
		 /// The commission run steps.
		 /// </value>
		 List<ICommissionRunStep> CommissionRunSteps { get; set; }

		 /// <summary>
		 /// Gets the commission run step number.
		 /// </summary>
		 /// <value>
		 /// The commission run step number.
		 /// </value>
		 int CommissionRunStepNumber { get; }

		 /// <summary>
		 /// Gets or sets the kind of the commission run.
		 /// </summary>
		 /// <value>
		 /// The kind of the commission run.
		 /// </value>
		 CommissionRunKind CommissionRunKind { get; set; }

		 /// <summary>
		 /// Gets or sets the commission run kind identifier.
		 /// </summary>
		 /// <value>
		 /// The commission run kind identifier.
		 /// </value>
		 int CommissionRunKindId { get; set; }

		 /// <summary>
		 /// Gets the current commission run progress.
		 /// </summary>
		 /// <value>
		 /// The current commission run progress.
		 /// </value>
		 string CurrentCommissionRunProgress { get; }

		 /// <summary>
		 /// Gets or sets the current commission run step.
		 /// </summary>
		 /// <value>
		 /// The current commission run step.
		 /// </value>
		 ICommissionRunStep CurrentCommissionRunStep { get; set; }

		 /// <summary>
		 /// Gets or sets the default commission run plan.
		 /// </summary>
		 /// <value>
		 /// The default commission run plan.
		 /// </value>
		 DisbursementFrequencyKind DefaultCommissionRunPlan { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [enabled].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [enabled]; otherwise, <c>false</c>.
		 /// </value>
		 bool Enabled { get; set; }

		 /// <summary>
		 /// Gets or sets a value indicating whether [in progress].
		 /// </summary>
		 /// <value>
		 ///   <c>true</c> if [in progress]; otherwise, <c>false</c>.
		 /// </value>
		 bool InProgress { get; set; }

		 /// <summary>
		 /// Gets or sets the last system event log identifier.
		 /// </summary>
		 /// <value>
		 /// The last system event log identifier.
		 /// </value>
		 int LastSystemEventLogId { get; set; }

		 /// <summary>
		 /// Gets or sets the next commission run step.
		 /// </summary>
		 /// <value>
		 /// The next commission run step.
		 /// </value>
		 ICommissionRunStep NextCommissionRunStep { get; set; }

		 /// <summary>
		 /// Gets or sets the open periods.
		 /// </summary>
		 /// <value>
		 /// The open periods.
		 /// </value>
		 IEnumerable<IPeriod> OpenPeriods { get; set; }

		 /// <summary>
		 /// Gets or sets the period identifier.
		 /// </summary>
		 /// <value>
		 /// The period identifier.
		 /// </value>
		 int PeriodId { get; set; }

		 /// <summary>
		 /// Gets or sets the plan identifier.
		 /// </summary>
		 /// <value>
		 /// The plan identifier.
		 /// </value>
		 int PlanId { get; set; }

		 /// <summary>
		 /// Gets or sets the system event identifier.
		 /// </summary>
		 /// <value>
		 /// The system event identifier.
		 /// </value>
		 int SystemEventId { get; set; }

		 /// <summary>
		 /// Gets or sets the system event log.
		 /// </summary>
		 /// <value>
		 /// The system event log.
		 /// </value>
		 ISystemEventLog SystemEventLog { get; set; }

		 /// <summary>
		 /// Gets or sets the system event setting.
		 /// </summary>
		 /// <value>
		 /// The system event setting.
		 /// </value>
		 ISystemEventSetting SystemEventSetting { get; set; }

		 /// <summary>
		 /// Gets the total commission run progress.
		 /// </summary>
		 /// <value>
		 /// The total commission run progress.
		 /// </value>
		 int TotalCommissionRunProgress { get; }

		 /// <summary>
		 /// Gets the total commission run steps.
		 /// </summary>
		 /// <value>
		 /// The total commission run steps.
		 /// </value>
		 int TotalCommissionRunSteps { get; }
	}
}
