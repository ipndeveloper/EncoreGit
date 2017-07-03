using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commissions override reason
	/// </summary>
	public interface IOverrideReason
	{
		/// <summary>
		/// Gets the override reason identifier
		/// </summary>
		int OverrideReasonId { get; }

		/// <summary>
		/// Gets the reason code
		/// </summary>
		string ReasonCode { get; }

		/// <summary>
		/// Gets the friendly name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the related override reason source
		/// </summary>
		IOverrideReasonSource OverrideReasonSource { get; }

		/// <summary>
		/// Gets the term name
		/// </summary>
		string TermName { get; }
	}
}
