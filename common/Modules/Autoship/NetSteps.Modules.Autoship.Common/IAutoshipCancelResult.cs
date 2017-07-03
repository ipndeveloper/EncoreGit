using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Result object for Autoship Cancel
	/// </summary>
	[DTO]
	public interface IAutoshipCancelResult
	{
		/// <summary>
		/// bool for if Cancel was successful
		/// </summary>
		bool Success { get; set; }
		/// <summary>
		/// Account for the autoship that was canceled
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// Autoship that was canceled
		/// </summary>
		int AutoshipID { get; set; }
	}
}
