using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Specification parameters for searching the downline.
	/// </summary>
	[DTO]
	public interface ISearchDownlineContext
	{
		/// <summary>
		/// The root account ID to begin the downline search.
		/// </summary>
		int RootAccountId { get; set; }

		/// <summary>
		/// A string containing one or more parts of the name(s) or account number(s) to search for.
		/// </summary>
		string Query { get; set; }
	}
}
