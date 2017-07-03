using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for gui_GetCommissionRunStatus_Result.
	/// </summary>
	public interface Igui_GetCommissionRunStatus_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProcessId for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string ProcessId { get; set; }
	
		/// <summary>
		/// The ProcessName for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string ProcessName { get; set; }
	
		/// <summary>
		/// The ProcessDesc for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string ProcessDesc { get; set; }
	
		/// <summary>
		/// The Status for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string Status { get; set; }
	
		/// <summary>
		/// The StartTime for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string StartTime { get; set; }
	
		/// <summary>
		/// The LastRunTime for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		string LastRunTime { get; set; }
	
		/// <summary>
		/// The RunOrder for this gui_GetCommissionRunStatus_Result.
		/// </summary>
		Nullable<int> RunOrder { get; set; }

	    #endregion
	}
}
