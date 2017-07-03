using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Kinds of commission runs available
	/// </summary>
	public enum CommissionRunKind
	{
		/// <summary>
		/// Defines a prep run
		/// </summary>
		Prep = 1,
		/// <summary>
		/// Defines a run intended to go live
		/// </summary>
		Publish = 2
	}
}
