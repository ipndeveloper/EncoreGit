using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Ledger entry reasons
	/// </summary>
	public interface ILedgerEntryReason
	{
		/// <summary>
		/// Gets or sets the entry reason identifier.
		/// </summary>
		/// <value>
		/// The entry reason identifier.
		/// </value>
		int EntryReasonId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is enabled]; otherwise, <c>false</c>.
		/// </value>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is editable].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is editable]; otherwise, <c>false</c>.
		/// </value>
		bool IsEditable { get; set; }
		/// <summary>
		/// Gets the name of the term.
		/// </summary>
		/// <value>
		/// The name of the term.
		/// </value>
		string TermName { get; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		string Code { get; set; }
	}
}
