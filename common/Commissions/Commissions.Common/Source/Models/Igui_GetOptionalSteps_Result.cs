using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for gui_GetOptionalSteps_Result.
	/// </summary>
	public interface Igui_GetOptionalSteps_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StepID for this gui_GetOptionalSteps_Result.
		/// </summary>
		int StepID { get; set; }
	
		/// <summary>
		/// The StepCode for this gui_GetOptionalSteps_Result.
		/// </summary>
		string StepCode { get; set; }

	    #endregion
	}
}
