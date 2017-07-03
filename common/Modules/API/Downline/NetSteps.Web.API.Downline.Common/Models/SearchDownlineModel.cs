using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Downline.Common.Models
{
	/// <summary>
	/// Search Downline view model
	/// </summary>
	public class SearchDownlineModel
	{
		/// <summary>
		/// Sponsor whos downline you want to search.
		/// </summary>
		public int SponsorID { get; set; }
		/// <summary>
		/// AccountID to search for.
		/// </summary>
		public int? AccountID { get; set; }
		/// <summary>
		/// Name or accountID to search for
		/// </summary>
		public string Query { get; set; }
		/// <summary>
		/// Account Type to search for
		/// </summary>
		public int? AccountTypeID { get; set; }
		/// <summary>
		/// Date an account was enrolled on or after
		/// </summary>
		public DateTime EnrollmentDate { get; set; }
		/// <summary>
		/// Flag to get only active accounts or all accounts.
		/// </summary>
		bool AllAccountStatuses { get; set; }
	}
}
