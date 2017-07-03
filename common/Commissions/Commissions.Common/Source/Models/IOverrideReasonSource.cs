using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Commissions overridee reason source
	/// </summary>
	public interface IOverrideReasonSource
	{
		/// <summary>
		/// Gets the override reason source identifier
		/// </summary>
		int OverrideReasonSourceId { get; }

		/// <summary>
		/// Gets the reason code
		/// </summary>
		string Code { get; }

		/// <summary>
		/// Gets the reason description
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Gets the term name
		/// </summary>
		string TermName { get; }

		/// <summary>
		/// Gets the friendly name
		/// </summary>
		string Name { get; }
	}
}
