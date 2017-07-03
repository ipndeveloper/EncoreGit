using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for gui_GetPlans_Result.
	/// </summary>
	public interface Igui_GetPlans_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PlanID for this gui_GetPlans_Result.
		/// </summary>
		int PlanID { get; set; }
	
		/// <summary>
		/// The PlanName for this gui_GetPlans_Result.
		/// </summary>
		string PlanName { get; set; }

	    #endregion
	}
}
