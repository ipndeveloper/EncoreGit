using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for Disbursement.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementContracts))]
	public interface IDisbursement
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementID for this Disbursement.
		/// </summary>
		int DisbursementID { get; set; }
	
		/// <summary>
		/// The Amount for this Disbursement.
		/// </summary>
		decimal Amount { get; set; }
	
		/// <summary>
		/// The DisbursementDate for this Disbursement.
		/// </summary>
		System.DateTime DisbursementDate { get; set; }
	
		/// <summary>
		/// The DisbursementStatusID for this Disbursement.
		/// </summary>
		int DisbursementStatusID { get; set; }
	
		/// <summary>
		/// The AccountID for this Disbursement.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The PeriodID for this Disbursement.
		/// </summary>
		Nullable<int> PeriodID { get; set; }
	
		/// <summary>
		/// The DisbursementProfileID for this Disbursement.
		/// </summary>
		Nullable<int> DisbursementProfileID { get; set; }
	
		/// <summary>
		/// The CurrencyTypeID for this Disbursement.
		/// </summary>
		Nullable<int> CurrencyTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this Disbursement.
		/// </summary>
	    ICommissionsAccount Account { get; set; }
	
		/// <summary>
		/// The DisbursementProfile for this Disbursement.
		/// </summary>
	    IDisbursementProfile DisbursementProfile { get; set; }
	
		/// <summary>
		/// The DisbursementStatus for this Disbursement.
		/// </summary>
	    IDisbursementStatus DisbursementStatus { get; set; }
	
		/// <summary>
		/// The Period for this Disbursement.
		/// </summary>
	    IPeriod Period { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountLedgers for this Disbursement.
		/// </summary>
		IEnumerable<IAccountLedger> AccountLedgers { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountLedger"/> to the AccountLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLedger"/> to add.</param>
		void AddAccountLedger(IAccountLedger item);
	
		/// <summary>
		/// Removes an <see cref="IAccountLedger"/> from the AccountLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLedger"/> to remove.</param>
		void RemoveAccountLedger(IAccountLedger item);
	
		/// <summary>
		/// The DisbursementDetails for this Disbursement.
		/// </summary>
		IEnumerable<IDisbursementDetail> DisbursementDetails { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementDetail"/> to the DisbursementDetails collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementDetail"/> to add.</param>
		void AddDisbursementDetail(IDisbursementDetail item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementDetail"/> from the DisbursementDetails collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementDetail"/> to remove.</param>
		void RemoveDisbursementDetail(IDisbursementDetail item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursement))]
		internal abstract class DisbursementContracts : IDisbursement
		{
		    #region Primitive properties
		
			int IDisbursement.DisbursementID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IDisbursement.Amount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IDisbursement.DisbursementDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursement.DisbursementStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursement.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursement.PeriodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursement.DisbursementProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursement.CurrencyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICommissionsAccount IDisbursement.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementProfile IDisbursement.DisbursementProfile
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementStatus IDisbursement.DisbursementStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IPeriod IDisbursement.Period
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountLedger> IDisbursement.AccountLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursement.AddAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursement.RemoveAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementDetail> IDisbursement.DisbursementDetails
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDisbursement.AddDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDisbursement.RemoveDisbursementDetail(IDisbursementDetail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
