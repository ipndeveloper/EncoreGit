using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for AccountLedger.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountLedgerContracts))]
	public interface IAccountLedger
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this AccountLedger.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The EntryID for this AccountLedger.
		/// </summary>
		int EntryID { get; set; }
	
		/// <summary>
		/// The EntryDescription for this AccountLedger.
		/// </summary>
		string EntryDescription { get; set; }
	
		/// <summary>
		/// The EntryReasonID for this AccountLedger.
		/// </summary>
		int EntryReasonID { get; set; }
	
		/// <summary>
		/// The EntryOriginID for this AccountLedger.
		/// </summary>
		int EntryOriginID { get; set; }
	
		/// <summary>
		/// The EntryTypeID for this AccountLedger.
		/// </summary>
		int EntryTypeID { get; set; }
	
		/// <summary>
		/// The UserID for this AccountLedger.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The EntryNotes for this AccountLedger.
		/// </summary>
		string EntryNotes { get; set; }
	
		/// <summary>
		/// The EntryAmount for this AccountLedger.
		/// </summary>
		decimal EntryAmount { get; set; }
	
		/// <summary>
		/// The EntryDate for this AccountLedger.
		/// </summary>
		System.DateTime EntryDate { get; set; }
	
		/// <summary>
		/// The EffectiveDate for this AccountLedger.
		/// </summary>
		System.DateTime EffectiveDate { get; set; }
	
		/// <summary>
		/// The BonusTypeID for this AccountLedger.
		/// </summary>
		Nullable<int> BonusTypeID { get; set; }
	
		/// <summary>
		/// The DisbursementID for this AccountLedger.
		/// </summary>
		Nullable<int> DisbursementID { get; set; }
	
		/// <summary>
		/// The EndingBalance for this AccountLedger.
		/// </summary>
		Nullable<decimal> EndingBalance { get; set; }
	
		/// <summary>
		/// The BonusValueID for this AccountLedger.
		/// </summary>
		Nullable<int> BonusValueID { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this AccountLedger.
		/// </summary>
		int CurrencyTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountLedger.
		/// </summary>
	    ICommissionsAccount Account { get; set; }
	
		/// <summary>
		/// The BonusType for this AccountLedger.
		/// </summary>
	    IBonusType BonusType { get; set; }
	
		/// <summary>
		/// The Disbursement for this AccountLedger.
		/// </summary>
	    IDisbursement Disbursement { get; set; }
	
		/// <summary>
		/// The LedgerEntryOrigin for this AccountLedger.
		/// </summary>
	    ILedgerEntryOrigin LedgerEntryOrigin { get; set; }
	
		/// <summary>
		/// The LedgerEntryReason for this AccountLedger.
		/// </summary>
	    ILedgerEntryReason LedgerEntryReason { get; set; }
	
		/// <summary>
		/// The LedgerEntryType for this AccountLedger.
		/// </summary>
	    ILedgerEntryType LedgerEntryType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLedger))]
		internal abstract class AccountLedgerContracts : IAccountLedger
		{
		    #region Primitive properties
		
			int IAccountLedger.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.EntryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountLedger.EntryDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.EntryReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.EntryOriginID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.EntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountLedger.EntryNotes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IAccountLedger.EntryAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAccountLedger.EntryDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAccountLedger.EffectiveDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountLedger.BonusTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountLedger.DisbursementID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IAccountLedger.EndingBalance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountLedger.BonusValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLedger.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICommissionsAccount IAccountLedger.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IBonusType IAccountLedger.BonusType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursement IAccountLedger.Disbursement
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryOrigin IAccountLedger.LedgerEntryOrigin
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryReason IAccountLedger.LedgerEntryReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryType IAccountLedger.LedgerEntryType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
