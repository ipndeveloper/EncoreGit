using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Override kind (formerly type)
	/// </summary>
	public interface IOverrideKind
	{
		/// <summary>
		/// the override kind identifier
		/// </summary>
		int OverrideKindId { get; }

		/// <summary>
		/// the override code
		/// </summary>
		string OverrideCode { get; }

		/// <summary>
		/// the override description
		/// </summary>
		string Description { get; }

		/// <summary>
		/// the override operator
		/// </summary>
		string Operator { get; }
	}
}
