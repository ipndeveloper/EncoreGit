using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for gui_GetPeriodsByPlan_Result.
	/// </summary>
	public interface Igui_GetPeriodsByPlan_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PeriodID for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		int PeriodID { get; set; }
	
		/// <summary>
		/// The EndDate for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		System.DateTime EndDate { get; set; }
	
		/// <summary>
		/// The PeriodName for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		string PeriodName { get; set; }
	
		/// <summary>
		/// The PeriodStartDate for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		Nullable<System.DateTime> PeriodStartDate { get; set; }
	
		/// <summary>
		/// The PeriodEndDate for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		Nullable<System.DateTime> PeriodEndDate { get; set; }
	
		/// <summary>
		/// The PeriodClosedFg for this gui_GetPeriodsByPlan_Result.
		/// </summary>
		int PeriodClosedFg { get; set; }

	    #endregion
	}
}
