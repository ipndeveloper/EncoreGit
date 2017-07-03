using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for uspGetPeriodsByAccountID_Result.
	/// </summary>
	public interface IuspGetPeriodsByAccountID_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PeriodID for this uspGetPeriodsByAccountID_Result.
		/// </summary>
		int PeriodID { get; set; }

	    #endregion
	}
}
