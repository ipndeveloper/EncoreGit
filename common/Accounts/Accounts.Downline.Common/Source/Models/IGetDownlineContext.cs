using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Specification parameters for getting downline data.
	/// </summary>
	[DTO]
	public interface IGetDownlineContext
	{
		/// <summary>
		/// The root account ID to begin the downline query.
		/// </summary>
		int RootAccountId { get; set; }

		/// <summary>
		/// If specified, the maximum number of levels in the downline to include.
		/// </summary>
		int? MaxLevels { get; set; }

		/// <summary>
		/// If specified, the account status IDs to include.
		/// </summary>
		IEnumerable<short> AccountStatusIds { get; set; }

		/// <summary>
		/// If specified, the account type IDs to include.
		/// </summary>
		IEnumerable<short> AccountTypeIds { get; set; }
	}
}
