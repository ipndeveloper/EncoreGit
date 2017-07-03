using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Search Downline model
	/// </summary>
	[DTO]
	public interface ISearchDownlineModel
	{
		/// <summary>
		/// Sponsor whos downline you want to search.
		/// </summary>
		int SponsorID { get; set; }
		/// <summary>
		/// AccountID to search for.
		/// </summary>
		int? AccountID { get; set; }
		/// <summary>
		/// Name or accountID to search for
		/// </summary>
		string Query { get; set; }
		/// <summary>
		/// Account Type to search for
		/// </summary>
		int? AccountTypeID { get; set; }
		/// <summary>
		/// Date an account was enrolled on or after
		/// </summary>
		DateTime EnrollmentDate { get; set; }
		/// <summary>
		/// Flag to get only active accounts or all accounts.
		/// </summary>
		bool AllAccountStatuses { get; set; }
	}
}
