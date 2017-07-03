using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commission Run Output
	/// </summary>
	public interface ICommissionRunOutput
	{
		/// <summary>
		/// Gets or sets the disbursement frequency.
		/// </summary>
		/// <value>
		/// The disbursement frequency.
		/// </value>
		DisbursementFrequencyKind DisbursementFrequency { get; set; }
		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>
		/// The results.
		/// </value>
		List<string> Results { get; set; }
	}
}
