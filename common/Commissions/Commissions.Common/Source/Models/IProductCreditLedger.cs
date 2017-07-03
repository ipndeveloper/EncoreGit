using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for ProductCreditLedger.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductCreditLedgerContracts))]
	public interface IProductCreditLedger
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this ProductCreditLedger.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The EntryID for this ProductCreditLedger.
		/// </summary>
		int EntryID { get; set; }
	
		/// <summary>
		/// The EntryDescription for this ProductCreditLedger.
		/// </summary>
		string EntryDescription { get; set; }
	
		/// <summary>
		/// The EntryReasonID for this ProductCreditLedger.
		/// </summary>
		int EntryReasonID { get; set; }
	
		/// <summary>
		/// The EntryOriginID for this ProductCreditLedger.
		/// </summary>
		int EntryOriginID { get; set; }
	
		/// <summary>
		/// The EntryTypeID for this ProductCreditLedger.
		/// </summary>
		int EntryTypeID { get; set; }
	
		/// <summary>
		/// The UserID for this ProductCreditLedger.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The EntryNotes for this ProductCreditLedger.
		/// </summary>
		string EntryNotes { get; set; }
	
		/// <summary>
		/// The EntryAmount for this ProductCreditLedger.
		/// </summary>
		decimal EntryAmount { get; set; }
	
		/// <summary>
		/// The EntryDate for this ProductCreditLedger.
		/// </summary>
		System.DateTime EntryDate { get; set; }
	
		/// <summary>
		/// The EffectiveDate for this ProductCreditLedger.
		/// </summary>
		System.DateTime EffectiveDate { get; set; }
	
		/// <summary>
		/// The BonusTypeID for this ProductCreditLedger.
		/// </summary>
		Nullable<int> BonusTypeID { get; set; }
	
		/// <summary>
		/// The BonusValueID for this ProductCreditLedger.
		/// </summary>
		Nullable<int> BonusValueID { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this ProductCreditLedger.
		/// </summary>
		int CurrencyTypeID { get; set; }
	
		/// <summary>
		/// The OrderID for this ProductCreditLedger.
		/// </summary>
		Nullable<int> OrderID { get; set; }
	
		/// <summary>
		/// The OrderPaymentID for this ProductCreditLedger.
		/// </summary>
		Nullable<int> OrderPaymentID { get; set; }
	
		/// <summary>
		/// The EndingBalance for this ProductCreditLedger.
		/// </summary>
		Nullable<decimal> EndingBalance { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this ProductCreditLedger.
		/// </summary>
	    ICommissionsAccount Account { get; set; }
	
		/// <summary>
		/// The BonusType for this ProductCreditLedger.
		/// </summary>
	    IBonusType BonusType { get; set; }
	
		/// <summary>
		/// The LedgerEntryOrigin for this ProductCreditLedger.
		/// </summary>
	    ILedgerEntryOrigin LedgerEntryOrigin { get; set; }
	
		/// <summary>
		/// The LedgerEntryReason for this ProductCreditLedger.
		/// </summary>
	    ILedgerEntryReason LedgerEntryReason { get; set; }
	
		/// <summary>
		/// The LedgerEntryType for this ProductCreditLedger.
		/// </summary>
	    ILedgerEntryType LedgerEntryType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductCreditLedger))]
		internal abstract class ProductCreditLedgerContracts : IProductCreditLedger
		{
		    #region Primitive properties
		
			int IProductCreditLedger.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.EntryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductCreditLedger.EntryDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.EntryReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.EntryOriginID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.EntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductCreditLedger.EntryNotes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IProductCreditLedger.EntryAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IProductCreditLedger.EntryDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IProductCreditLedger.EffectiveDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductCreditLedger.BonusTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductCreditLedger.BonusValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductCreditLedger.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductCreditLedger.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductCreditLedger.OrderPaymentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IProductCreditLedger.EndingBalance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICommissionsAccount IProductCreditLedger.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IBonusType IProductCreditLedger.BonusType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryOrigin IProductCreditLedger.LedgerEntryOrigin
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryReason IProductCreditLedger.LedgerEntryReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ILedgerEntryType IProductCreditLedger.LedgerEntryType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
