using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for CommissionsAccount.
	/// </summary>
	[ContractClass(typeof(Contracts.CommissionsAccountContracts))]
	public interface ICommissionsAccount
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this Account.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AccountNumber for this Account.
		/// </summary>
		string AccountNumber { get; set; }
	
		/// <summary>
		/// The AccountTypeID for this Account.
		/// </summary>
		int AccountTypeID { get; set; }
	
		/// <summary>
		/// The FirstName for this Account.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The MiddleName for this Account.
		/// </summary>
		string MiddleName { get; set; }
	
		/// <summary>
		/// The LastName for this Account.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The EmailAddress for this Account.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The SponsorID for this Account.
		/// </summary>
		Nullable<int> SponsorID { get; set; }
	
		/// <summary>
		/// The EnrollerID for this Account.
		/// </summary>
		Nullable<int> EnrollerID { get; set; }
	
		/// <summary>
		/// The EnrollmentDateUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> EnrollmentDateUTC { get; set; }
	
		/// <summary>
		/// The DataVersion for this Account.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The IsTaxExempt for this Account.
		/// </summary>
		Nullable<bool> IsTaxExempt { get; set; }
	
		/// <summary>
		/// The TaxNumber for this Account.
		/// </summary>
		string TaxNumber { get; set; }
	
		/// <summary>
		/// The IsEntity for this Account.
		/// </summary>
		bool IsEntity { get; set; }
	
		/// <summary>
		/// The AccountStatusChangeReasonID for this Account.
		/// </summary>
		Nullable<int> AccountStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The AccountStatusID for this Account.
		/// </summary>
		Nullable<int> AccountStatusID { get; set; }
	
		/// <summary>
		/// The EntityName for this Account.
		/// </summary>
		string EntityName { get; set; }
	
		/// <summary>
		/// The CoApplicant for this Account.
		/// </summary>
		string CoApplicant { get; set; }
	
		/// <summary>
		/// The BusinessCenterOwnerID for this Account.
		/// </summary>
		Nullable<int> BusinessCenterOwnerID { get; set; }
	
		/// <summary>
		/// The CountryID for this Account.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The LastRenewalUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> LastRenewalUTC { get; set; }
	
		/// <summary>
		/// The NextRenewalUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> NextRenewalUTC { get; set; }
	
		/// <summary>
		/// The BirthdayUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> BirthdayUTC { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountLedgers for this Account.
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
		/// The AccountTitleOverrides for this Account.
		/// </summary>
		IEnumerable<IAccountTitleOverride> AccountTitleOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTitleOverride"/> to the AccountTitleOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitleOverride"/> to add.</param>
		void AddAccountTitleOverride(IAccountTitleOverride item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTitleOverride"/> from the AccountTitleOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitleOverride"/> to remove.</param>
		void RemoveAccountTitleOverride(IAccountTitleOverride item);
	
		/// <summary>
		/// The AccountTitles for this Account.
		/// </summary>
		IEnumerable<IAccountTitle> AccountTitles { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTitle"/> to the AccountTitles collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitle"/> to add.</param>
		void AddAccountTitle(IAccountTitle item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTitle"/> from the AccountTitles collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTitle"/> to remove.</param>
		void RemoveAccountTitle(IAccountTitle item);
	
		/// <summary>
		/// The CalculationOverrides for this Account.
		/// </summary>
		IEnumerable<ICalculationOverride> CalculationOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="ICalculationOverride"/> to the CalculationOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculationOverride"/> to add.</param>
		void AddCalculationOverride(ICalculationOverride item);
	
		/// <summary>
		/// Removes an <see cref="ICalculationOverride"/> from the CalculationOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculationOverride"/> to remove.</param>
		void RemoveCalculationOverride(ICalculationOverride item);
	
		/// <summary>
		/// The Calculations for this Account.
		/// </summary>
		IEnumerable<ICalculation> Calculations { get; }
	
		/// <summary>
		/// Adds an <see cref="ICalculation"/> to the Calculations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculation"/> to add.</param>
		void AddCalculation(ICalculation item);
	
		/// <summary>
		/// Removes an <see cref="ICalculation"/> from the Calculations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICalculation"/> to remove.</param>
		void RemoveCalculation(ICalculation item);
	
		/// <summary>
		/// The CheckHolds for this Account.
		/// </summary>
		IEnumerable<ICheckHold> CheckHolds { get; }
	
		/// <summary>
		/// Adds an <see cref="ICheckHold"/> to the CheckHolds collection.
		/// </summary>
		/// <param name="item">The <see cref="ICheckHold"/> to add.</param>
		void AddCheckHold(ICheckHold item);
	
		/// <summary>
		/// Removes an <see cref="ICheckHold"/> from the CheckHolds collection.
		/// </summary>
		/// <param name="item">The <see cref="ICheckHold"/> to remove.</param>
		void RemoveCheckHold(ICheckHold item);
	
		/// <summary>
		/// The DisbursementProfiles for this Account.
		/// </summary>
		IEnumerable<IDisbursementProfile> DisbursementProfiles { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursementProfile"/> to the DisbursementProfiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfile"/> to add.</param>
		void AddDisbursementProfile(IDisbursementProfile item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursementProfile"/> from the DisbursementProfiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursementProfile"/> to remove.</param>
		void RemoveDisbursementProfile(IDisbursementProfile item);
	
		/// <summary>
		/// The Disbursements for this Account.
		/// </summary>
		IEnumerable<IDisbursement> Disbursements { get; }
	
		/// <summary>
		/// Adds an <see cref="IDisbursement"/> to the Disbursements collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursement"/> to add.</param>
		void AddDisbursement(IDisbursement item);
	
		/// <summary>
		/// Removes an <see cref="IDisbursement"/> from the Disbursements collection.
		/// </summary>
		/// <param name="item">The <see cref="IDisbursement"/> to remove.</param>
		void RemoveDisbursement(IDisbursement item);
	
		/// <summary>
		/// The ProductCreditLedgers for this Account.
		/// </summary>
		IEnumerable<IProductCreditLedger> ProductCreditLedgers { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductCreditLedger"/> to the ProductCreditLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductCreditLedger"/> to add.</param>
		void AddProductCreditLedger(IProductCreditLedger item);
	
		/// <summary>
		/// Removes an <see cref="IProductCreditLedger"/> from the ProductCreditLedgers collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductCreditLedger"/> to remove.</param>
		void RemoveProductCreditLedger(IProductCreditLedger item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICommissionsAccount))]
		internal abstract class CommissionsAccountContracts : ICommissionsAccount
		{
		    #region Primitive properties
		
			int ICommissionsAccount.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.AccountNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICommissionsAccount.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.MiddleName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICommissionsAccount.SponsorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICommissionsAccount.EnrollerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICommissionsAccount.EnrollmentDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] ICommissionsAccount.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> ICommissionsAccount.IsTaxExempt
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.TaxNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICommissionsAccount.IsEntity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICommissionsAccount.AccountStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICommissionsAccount.AccountStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.EntityName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICommissionsAccount.CoApplicant
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICommissionsAccount.BusinessCenterOwnerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICommissionsAccount.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICommissionsAccount.LastRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICommissionsAccount.NextRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICommissionsAccount.BirthdayUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountLedger> ICommissionsAccount.AccountLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveAccountLedger(IAccountLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTitleOverride> ICommissionsAccount.AccountTitleOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddAccountTitleOverride(IAccountTitleOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveAccountTitleOverride(IAccountTitleOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTitle> ICommissionsAccount.AccountTitles
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddAccountTitle(IAccountTitle item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveAccountTitle(IAccountTitle item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICalculationOverride> ICommissionsAccount.CalculationOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveCalculationOverride(ICalculationOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICalculation> ICommissionsAccount.Calculations
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddCalculation(ICalculation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveCalculation(ICalculation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICheckHold> ICommissionsAccount.CheckHolds
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddCheckHold(ICheckHold item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveCheckHold(ICheckHold item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursementProfile> ICommissionsAccount.DisbursementProfiles
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddDisbursementProfile(IDisbursementProfile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveDisbursementProfile(IDisbursementProfile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDisbursement> ICommissionsAccount.Disbursements
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveDisbursement(IDisbursement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductCreditLedger> ICommissionsAccount.ProductCreditLedgers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICommissionsAccount.AddProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICommissionsAccount.RemoveProductCreditLedger(IProductCreditLedger item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
