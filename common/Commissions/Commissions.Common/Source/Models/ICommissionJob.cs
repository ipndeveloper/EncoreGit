using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission job
	/// </summary>
	public interface ICommissionJob
	{
		/// <summary>
		/// Gets or sets the commission run identifier.
		/// </summary>
		/// <value>
		/// The commission run identifier.
		/// </value>
		int CommissionRunId { get; set; }
		/// <summary>
		/// Gets or sets the disbursement frequency.
		/// </summary>
		/// <value>
		/// The disbursement frequency.
		/// </value>
		DisbursementFrequencyKind DisbursementFrequency { get; set; }
		/// <summary>
		/// Gets or sets the type of the commission run.
		/// </summary>
		/// <value>
		/// The type of the commission run.
		/// </value>
		CommissionRunKind CommissionRunType { get; set; }

		/// <summary>
		/// Gets or sets the name of the job.
		/// </summary>
		/// <value>
		/// The name of the job.
		/// </value>
		string JobName { get; set; }
		/// <summary>
		/// Gets or sets the display name of the job.
		/// </summary>
		/// <value>
		/// The display name of the job.
		/// </value>
		string JobDisplayName { get; set; }
	}
}
