using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Disbursement plan kinds
	/// </summary>
	public enum DisbursementFrequencyKind
	{
		/// <summary>
		/// Commission run monthly
		/// </summary>
		Monthly = 1,
		/// <summary>
		/// Commission run weekly
		/// </summary>
		Weekly = 2,
		/// <summary>
		/// Commission run daily
		/// </summary>
		Daily = 3,
		/// <summary>
		/// Commission rerun - ??
		/// </summary>
		Rerun = 4
	}
}
