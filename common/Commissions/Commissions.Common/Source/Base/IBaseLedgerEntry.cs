using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Base
{
	/// <summary>
	/// Base ledger entry
	/// </summary>
	public interface IBaseLedgerEntry
	{
		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		int AccountId { get; set; }
		/// <summary>
		/// Gets or sets the bonus type identifier.
		/// </summary>
		/// <value>
		/// The bonus type identifier.
		/// </value>
		int? BonusTypeId { get; set; }
		/// <summary>
		/// Gets or sets the bonus value identifier.
		/// </summary>
		/// <value>
		/// The bonus value identifier.
		/// </value>
		int? BonusValueId { get; set; }
		/// <summary>
		/// Gets or sets the currency type identifier.
		/// </summary>
		/// <value>
		/// The currency type identifier.
		/// </value>
		int CurrencyTypeId { get; set; }
		/// <summary>
		/// Gets or sets the effective date.
		/// </summary>
		/// <value>
		/// The effective date.
		/// </value>
		DateTime EffectiveDate { get; set; }
		/// <summary>
		/// Gets or sets the ending balance.
		/// </summary>
		/// <value>
		/// The ending balance.
		/// </value>
		decimal? EndingBalance { get; set; }
		/// <summary>
		/// Gets or sets the entry amount.
		/// </summary>
		/// <value>
		/// The entry amount.
		/// </value>
		decimal EntryAmount { get; set; }
		/// <summary>
		/// Gets or sets the entry date.
		/// </summary>
		/// <value>
		/// The entry date.
		/// </value>
		DateTime EntryDate { get; set; }
		/// <summary>
		/// Gets or sets the entry description.
		/// </summary>
		/// <value>
		/// The entry description.
		/// </value>
		string EntryDescription { get; set; }
		/// <summary>
		/// Gets or sets the entry identifier.
		/// </summary>
		/// <value>
		/// The entry identifier.
		/// </value>
		int EntryId { get; set; }
		/// <summary>
		/// Gets or sets the entry notes.
		/// </summary>
		/// <value>
		/// The entry notes.
		/// </value>
		string EntryNotes { get; set; }
		
		/// <summary>
		/// Gets or sets the entry origin identifier.
		/// </summary>
		/// <value>
		/// The entry origin identifier.
		/// </value>
		ILedgerEntryOrigin EntryOrigin { get; set; }
		
		/// <summary>
		/// Gets or sets the entry reason identifier.
		/// </summary>
		/// <value>
		/// The entry reason identifier.
		/// </value>
		ILedgerEntryReason EntryReason { get; set; }
		
		/// <summary>
		/// Gets or sets the entry type identifier.
		/// </summary>
		/// <value>
		/// The entry type identifier.
		/// </value>
		ILedgerEntryKind EntryKind { get; set; }
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		int UserId { get; set; }
	}
}
