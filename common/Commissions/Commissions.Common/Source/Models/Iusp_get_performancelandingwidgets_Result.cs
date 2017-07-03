using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for usp_get_performancelandingwidgets_Result.
	/// </summary>
	public interface Iusp_get_performancelandingwidgets_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CurrentLevel for this usp_get_performancelandingwidgets_Result.
		/// </summary>
		Nullable<int> CurrentLevel { get; set; }
	
		/// <summary>
		/// The PaidAsLevel for this usp_get_performancelandingwidgets_Result.
		/// </summary>
		Nullable<int> PaidAsLevel { get; set; }
	
		/// <summary>
		/// The SalesIndicatorLevel for this usp_get_performancelandingwidgets_Result.
		/// </summary>
		string SalesIndicatorLevel { get; set; }
	
		/// <summary>
		/// The Volume for this usp_get_performancelandingwidgets_Result.
		/// </summary>
		Nullable<decimal> Volume { get; set; }
	
		/// <summary>
		/// The RequiredVolume for this usp_get_performancelandingwidgets_Result.
		/// </summary>
		Nullable<decimal> RequiredVolume { get; set; }

	    #endregion
	}
}
