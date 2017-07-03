using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Modules.Autoship.Common;

namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Result object for autoship functions
	/// </summary>
	[DTO]
	public interface IAutoshipSearchResult
	{
		/// <summary>
		/// bool for if Cancel or Search was successful
		/// </summary>
		bool Success { get; set; }
		/// <summary>
		/// List of search results.
		/// </summary>
		IEnumerable<ISiteAutoship> Autoships { get; set; }
	}
}
