using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Title Kind (formerly type)
	/// </summary>
	public interface ITitleKind
	{
		/// <summary>
		/// The title kind identifier
		/// </summary>
		int TitleKindId { get; }

		/// <summary>
		/// the title kind code
		/// </summary>
		string TitleKindCode { get; }

		/// <summary>
		/// the friendly name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// the term name
		/// </summary>
		string TermName { get; }
	}
}
