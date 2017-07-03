using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for uspProcessInitialDisbursements_Result.
	/// </summary>
	public interface IuspProcessInitialDisbursements_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SystemEventID for this uspProcessInitialDisbursements_Result.
		/// </summary>
		int SystemEventID { get; set; }
	
		/// <summary>
		/// The SystemEventLogApplicationID for this uspProcessInitialDisbursements_Result.
		/// </summary>
		int SystemEventLogApplicationID { get; set; }
	
		/// <summary>
		/// The SystemEventLogTypeID for this uspProcessInitialDisbursements_Result.
		/// </summary>
		int SystemEventLogTypeID { get; set; }
	
		/// <summary>
		/// The EventMessage for this uspProcessInitialDisbursements_Result.
		/// </summary>
		string EventMessage { get; set; }
	
		/// <summary>
		/// The CreatedDate for this uspProcessInitialDisbursements_Result.
		/// </summary>
		Nullable<System.DateTime> CreatedDate { get; set; }

	    #endregion
	}
}
