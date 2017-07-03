using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Entry Kind (formerly type)
	/// </summary>
	public interface ILedgerEntryKind
	{
		/// <summary>
		/// Gets or sets the ledger entry kind identifier.
		/// </summary>
		/// <value>
		/// The ledger entry kind identifier.
		/// </value>
		int LedgerEntryKindId { get; set; }

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
		/// Gets or sets a value indicating whether [is taxable].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is taxable]; otherwise, <c>false</c>.
		/// </value>
		bool IsTaxable { get; set; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		string Code { get; set; }
		/// <summary>
		/// Gets the name of the term.
		/// </summary>
		/// <value>
		/// The name of the term.
		/// </value>
		string TermName { get; }
	}
}
