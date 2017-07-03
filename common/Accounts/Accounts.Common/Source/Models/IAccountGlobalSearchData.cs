using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountGlobalSearchData.
	/// </summary>
	public interface IAccountGlobalSearchData
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this AccountGlobalSearchData.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AccountNumber for this AccountGlobalSearchData.
		/// </summary>
		string AccountNumber { get; set; }
	
		/// <summary>
		/// The SponsorID for this AccountGlobalSearchData.
		/// </summary>
		Nullable<int> SponsorID { get; set; }
	
		/// <summary>
		/// The AccountTypeID for this AccountGlobalSearchData.
		/// </summary>
		short AccountTypeID { get; set; }
	
		/// <summary>
		/// The FirstName for this AccountGlobalSearchData.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this AccountGlobalSearchData.
		/// </summary>
		string LastName { get; set; }

	    #endregion
	}
}
