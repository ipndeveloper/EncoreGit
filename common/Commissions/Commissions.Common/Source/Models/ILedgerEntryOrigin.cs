using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Product Credit Ledger Origin
	/// </summary>
	public interface ILedgerEntryOrigin
	{
		/// <summary>
		/// Gets the entry origin identifier.
		/// </summary>
		/// <value>
		/// The entry origin identifier.
		/// </value>
		int EntryOriginId { get; }

		/// <summary>
		/// Gets the origin name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets a value indicating whether [is enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is enabled]; otherwise, <c>false</c>.
		/// </value>
		bool IsEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether [is editable].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is editable]; otherwise, <c>false</c>.
		/// </value>
		bool IsEditable { get; }

		/// <summary>
		/// Gets the origin term name
		/// </summary>
		string TermName { get; }

		/// <summary>
		/// Gets the origin code
		/// </summary>
		string Code { get; }
	}
}
