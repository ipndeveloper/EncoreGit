using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Downline Search results
	/// </summary>
	[DTO]
	public interface IDownlineSearchResult
	{
		/// <summary>
		/// List of IDownlineAccounts returned by search
		/// </summary>
		List<IDownlineAccount> DownlineAccounts { get; set; }
		/// <summary>
		/// if search was successfull
		/// </summary>
		bool Success { get; set; }
	}
}
