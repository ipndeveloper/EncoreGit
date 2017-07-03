using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for uspGetIncentiveForAccountIDIncentiveID_Result.
	/// </summary>
	public interface IuspGetIncentiveForAccountIDIncentiveID_Result
	{
	    #region Primitive properties
	
		/// <summary>
		/// The Incentive_Name for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		string Incentive_Name { get; set; }
	
		/// <summary>
		/// The Account_Title for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		string Account_Title { get; set; }
	
		/// <summary>
		/// The Total_Points for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		Nullable<decimal> Total_Points { get; set; }
	
		/// <summary>
		/// The Group_Volume for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		Nullable<int> Group_Volume { get; set; }
	
		/// <summary>
		/// The Percentage_of_Points_Earned_Toward_Incentive_Reward for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		Nullable<decimal> Percentage_of_Points_Earned_Toward_Incentive_Reward { get; set; }
	
		/// <summary>
		/// The Personal_Volume for this uspGetIncentiveForAccountIDIncentiveID_Result.
		/// </summary>
		Nullable<int> Personal_Volume { get; set; }

	    #endregion
	}
}
