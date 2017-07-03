using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AvailabilityLookup.Common
{
	/// <summary>
	/// Module lookup Result
	/// </summary>
	[DTO]
	public interface ILookupResult
	{
		/// <summary>
		/// bool for if site was found
		/// </summary>
		bool Success { get; set; }
		/// <summary>
		/// AccountID of the site if site was found.
		/// </summary>
		int AccountID { get; set; }
	}
}
