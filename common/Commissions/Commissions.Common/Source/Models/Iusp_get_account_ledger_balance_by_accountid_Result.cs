using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for usp_get_account_ledger_balance_by_accountid_Result.
	/// </summary>
	public interface Iusp_get_account_ledger_balance_by_accountid_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CurrentEndingBalance for this usp_get_account_ledger_balance_by_accountid_Result.
		/// </summary>
		Nullable<decimal> CurrentEndingBalance { get; set; }

	    #endregion
	}
}
