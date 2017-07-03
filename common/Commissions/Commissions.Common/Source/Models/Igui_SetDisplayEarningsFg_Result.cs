using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for gui_SetDisplayEarningsFg_Result.
	/// </summary>
	public interface Igui_SetDisplayEarningsFg_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EarningsViewable for this gui_SetDisplayEarningsFg_Result.
		/// </summary>
		Nullable<bool> EarningsViewable { get; set; }

	    #endregion
	}
}
