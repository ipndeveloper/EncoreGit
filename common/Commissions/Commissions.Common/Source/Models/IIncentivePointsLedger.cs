using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for IncentivePointsLedger.
	/// </summary>
	[ContractClass(typeof(Contracts.IncentivePointsLedgerContracts))]
	public interface IIncentivePointsLedger
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this IncentivePointsLedger.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The EntryID for this IncentivePointsLedger.
		/// </summary>
		int EntryID { get; set; }
	
		/// <summary>
		/// The EntryDescription for this IncentivePointsLedger.
		/// </summary>
		string EntryDescription { get; set; }
	
		/// <summary>
		/// The EntryReasonID for this IncentivePointsLedger.
		/// </summary>
		int EntryReasonID { get; set; }
	
		/// <summary>
		/// The EntryOriginID for this IncentivePointsLedger.
		/// </summary>
		int EntryOriginID { get; set; }
	
		/// <summary>
		/// The EntryTypeID for this IncentivePointsLedger.
		/// </summary>
		int EntryTypeID { get; set; }
	
		/// <summary>
		/// The UserID for this IncentivePointsLedger.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The EntryNotes for this IncentivePointsLedger.
		/// </summary>
		string EntryNotes { get; set; }
	
		/// <summary>
		/// The EntryAmount for this IncentivePointsLedger.
		/// </summary>
		decimal EntryAmount { get; set; }
	
		/// <summary>
		/// The EntryDate for this IncentivePointsLedger.
		/// </summary>
		System.DateTime EntryDate { get; set; }
	
		/// <summary>
		/// The EffectiveDate for this IncentivePointsLedger.
		/// </summary>
		System.DateTime EffectiveDate { get; set; }
	
		/// <summary>
		/// The BonusTypeID for this IncentivePointsLedger.
		/// </summary>
		Nullable<int> BonusTypeID { get; set; }
	
		/// <summary>
		/// The PeriodID for this IncentivePointsLedger.
		/// </summary>
		Nullable<int> PeriodID { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this IncentivePointsLedger.
		/// </summary>
		int CurrencyTypeID { get; set; }
	
		/// <summary>
		/// The IncentiveID for this IncentivePointsLedger.
		/// </summary>
		Nullable<int> IncentiveID { get; set; }
	
		/// <summary>
		/// The IncentiveQualID for this IncentivePointsLedger.
		/// </summary>
		Nullable<int> IncentiveQualID { get; set; }
	
		/// <summary>
		/// The EndingBalance for this IncentivePointsLedger.
		/// </summary>
		Nullable<decimal> EndingBalance { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IIncentivePointsLedger))]
		internal abstract class IncentivePointsLedgerContracts : IIncentivePointsLedger
		{
		    #region Primitive properties
		
			int IIncentivePointsLedger.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.EntryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentivePointsLedger.EntryDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.EntryReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.EntryOriginID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.EntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentivePointsLedger.EntryNotes
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IIncentivePointsLedger.EntryAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IIncentivePointsLedger.EntryDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IIncentivePointsLedger.EffectiveDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentivePointsLedger.BonusTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentivePointsLedger.PeriodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentivePointsLedger.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentivePointsLedger.IncentiveID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentivePointsLedger.IncentiveQualID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IIncentivePointsLedger.EndingBalance
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
