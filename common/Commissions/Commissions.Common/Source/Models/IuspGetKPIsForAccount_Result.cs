using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for uspGetKPIsForAccount_Result.
	/// </summary>
	public interface IuspGetKPIsForAccount_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The KPITypeCode for this uspGetKPIsForAccount_Result.
		/// </summary>
		string KPITypeCode { get; set; }
	
		/// <summary>
		/// The KPIValue for this uspGetKPIsForAccount_Result.
		/// </summary>
		string KPIValue { get; set; }
	
		/// <summary>
		/// The DataType for this uspGetKPIsForAccount_Result.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The TermName for this uspGetKPIsForAccount_Result.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	}
}
