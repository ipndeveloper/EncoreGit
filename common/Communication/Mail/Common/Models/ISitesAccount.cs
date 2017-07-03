using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for SitesAccount.
	/// </summary>
	[ContractClass(typeof(Contracts.SitesAccountContracts))]
	public interface ISitesAccount
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this SitesAccount.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AccountNumber for this SitesAccount.
		/// </summary>
		string AccountNumber { get; set; }
	
		/// <summary>
		/// The AccountTypeID for this SitesAccount.
		/// </summary>
		short AccountTypeID { get; set; }
	
		/// <summary>
		/// The AccountStatusID for this SitesAccount.
		/// </summary>
		short AccountStatusID { get; set; }
	
		/// <summary>
		/// The PreferedContactMethodID for this SitesAccount.
		/// </summary>
		Nullable<int> PreferedContactMethodID { get; set; }
	
		/// <summary>
		/// The DefaultLanguageID for this SitesAccount.
		/// </summary>
		int DefaultLanguageID { get; set; }
	
		/// <summary>
		/// The UserID for this SitesAccount.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The FirstName for this SitesAccount.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The MiddleName for this SitesAccount.
		/// </summary>
		string MiddleName { get; set; }
	
		/// <summary>
		/// The LastName for this SitesAccount.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The NickName for this SitesAccount.
		/// </summary>
		string NickName { get; set; }
	
		/// <summary>
		/// The CoApplicant for this SitesAccount.
		/// </summary>
		string CoApplicant { get; set; }
	
		/// <summary>
		/// The EmailAddress for this SitesAccount.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The SponsorID for this SitesAccount.
		/// </summary>
		Nullable<int> SponsorID { get; set; }
	
		/// <summary>
		/// The EnrollerID for this SitesAccount.
		/// </summary>
		Nullable<int> EnrollerID { get; set; }
	
		/// <summary>
		/// The EnrollmentDateUTC for this SitesAccount.
		/// </summary>
		Nullable<System.DateTime> EnrollmentDateUTC { get; set; }
	
		/// <summary>
		/// The IsTaxExempt for this SitesAccount.
		/// </summary>
		Nullable<bool> IsTaxExempt { get; set; }
	
		/// <summary>
		/// The TaxNumber for this SitesAccount.
		/// </summary>
		string TaxNumber { get; set; }
	
		/// <summary>
		/// The IsEntity for this SitesAccount.
		/// </summary>
		bool IsEntity { get; set; }
	
		/// <summary>
		/// The EntityName for this SitesAccount.
		/// </summary>
		string EntityName { get; set; }
	
		/// <summary>
		/// The AccountStatusChangeReasonID for this SitesAccount.
		/// </summary>
		Nullable<short> AccountStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The LastRenewalUTC for this SitesAccount.
		/// </summary>
		Nullable<System.DateTime> LastRenewalUTC { get; set; }
	
		/// <summary>
		/// The NextRenewalUTC for this SitesAccount.
		/// </summary>
		Nullable<System.DateTime> NextRenewalUTC { get; set; }
	
		/// <summary>
		/// The ReceivedApplication for this SitesAccount.
		/// </summary>
		bool ReceivedApplication { get; set; }
	
		/// <summary>
		/// The IsTaxExemptVerified for this SitesAccount.
		/// </summary>
		bool IsTaxExemptVerified { get; set; }
	
		/// <summary>
		/// The DateApplicationReceivedUTC for this SitesAccount.
		/// </summary>
		Nullable<System.DateTime> DateApplicationReceivedUTC { get; set; }
	
		/// <summary>
		/// The BirthdayUTC for this SitesAccount.
		/// </summary>
		Nullable<System.DateTime> BirthdayUTC { get; set; }
	
		/// <summary>
		/// The GenderID for this SitesAccount.
		/// </summary>
		Nullable<short> GenderID { get; set; }
	
		/// <summary>
		/// The DataVersion for this SitesAccount.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this SitesAccount.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this SitesAccount.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this SitesAccount.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The AccountSourceID for this SitesAccount.
		/// </summary>
		Nullable<short> AccountSourceID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISitesAccount))]
		internal abstract class SitesAccountContracts : ISitesAccount
		{
		    #region Primitive properties
		
			int ISitesAccount.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.AccountNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISitesAccount.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISitesAccount.AccountStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.PreferedContactMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISitesAccount.DefaultLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.MiddleName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.NickName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.CoApplicant
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.SponsorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.EnrollerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISitesAccount.EnrollmentDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> ISitesAccount.IsTaxExempt
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.TaxNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISitesAccount.IsEntity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISitesAccount.EntityName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ISitesAccount.AccountStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISitesAccount.LastRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISitesAccount.NextRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISitesAccount.ReceivedApplication
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISitesAccount.IsTaxExemptVerified
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISitesAccount.DateApplicationReceivedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISitesAccount.BirthdayUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ISitesAccount.GenderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] ISitesAccount.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISitesAccount.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISitesAccount.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ISitesAccount.AccountSourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
