using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Models;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Account.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountContracts))]
	public interface IAccount
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
		short AccountTypeID { get; set; }
	
		/// <summary>
		/// The AccountStatusID for this Account.
		/// </summary>
		short AccountStatusID { get; set; }
	
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
		/// The BirthdayUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> BirthdayUTC { get; set; }
	
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
		/// The IsTaxExempt for this Account.
		/// </summary>
		Nullable<bool> IsTaxExempt { get; set; }
	
		/// <summary>
		/// The TaxNumber for this Account.
		/// </summary>
		string TaxNumber { get; }
	
		/// <summary>
		/// The IsEntity for this Account.
		/// </summary>
		bool IsEntity { get; set; }
	
		/// <summary>
		/// The LastRenewalUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> LastRenewalUTC { get; set; }
	
		/// <summary>
		/// The ReceivedApplication for this Account.
		/// </summary>
		bool ReceivedApplication { get; set; }
	
		/// <summary>
		/// The NickName for this Account.
		/// </summary>
		string NickName { get; set; }
	
		/// <summary>
		/// The CoApplicant for this Account.
		/// </summary>
		string CoApplicant { get; set; }
	
		/// <summary>
		/// The EntityName for this Account.
		/// </summary>
		string EntityName { get; set; }
	
		/// <summary>
		/// The GenderID for this Account.
		/// </summary>
		Nullable<short> GenderID { get; set; }
	
		/// <summary>
		/// The DataVersion for this Account.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Account.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this Account.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this Account.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The UserID for this Account.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The DefaultLanguageID for this Account.
		/// </summary>
		int DefaultLanguageID { get; set; }
	
		/// <summary>
		/// The AccountStatusChangeReasonID for this Account.
		/// </summary>
		Nullable<short> AccountStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The PreferedContactMethodID for this Account.
		/// </summary>
		Nullable<int> PreferedContactMethodID { get; set; }
	
		/// <summary>
		/// The NextRenewalUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> NextRenewalUTC { get; set; }
	
		/// <summary>
		/// The IsTaxExemptVerified for this Account.
		/// </summary>
		bool IsTaxExemptVerified { get; set; }
	
		/// <summary>
		/// The AccountSourceID for this Account.
		/// </summary>
		Nullable<short> AccountSourceID { get; set; }
	
		/// <summary>
		/// The DateLastModifiedUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> DateLastModifiedUTC { get; set; }
	
		/// <summary>
		/// The TerminatedDateUTC for this Account.
		/// </summary>
		Nullable<System.DateTime> TerminatedDateUTC { get; set; }
	
		/// <summary>
		/// The TaxGeocode for this Account.
		/// </summary>
		string TaxGeocode { get; set; }
	
		/// <summary>
		/// The MarketID for this Account.
		/// </summary>
		int MarketID { get; set; }

		/// <summary>
		/// Indicates whether the user has opted out of communication or not
		/// </summary>
		bool IsOptedOut { get; set; }
	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account2 for this Account.
		/// </summary>
	    IAccount Account2 { get; set; }
	
		/// <summary>
		/// The AccountListValue for this Account.
		/// </summary>
	    IAccountListValue AccountListValue { get; set; }
	
		/// <summary>
		/// The AccountSource for this Account.
		/// </summary>
	    IAccountSource AccountSource { get; set; }
	
		/// <summary>
		/// The AccountStatus for this Account.
		/// </summary>
	    IAccountStatus AccountStatus { get; set; }
	
		/// <summary>
		/// The AccountStatusChangeReason for this Account.
		/// </summary>
	    IAccountStatusChangeReason AccountStatusChangeReason { get; set; }
	
		/// <summary>
		/// The AccountType for this Account.
		/// </summary>
	    IAccountType AccountType { get; set; }
	
		/// <summary>
		/// The CreatedByUser for this Account.
		/// </summary>
	    IUser CreatedByUser { get; set; }
	
		/// <summary>
		/// The Gender for this Account.
		/// </summary>
	    IGender Gender { get; set; }
	
		/// <summary>
		/// The ModifiedByUser for this Account.
		/// </summary>
	    IUser ModifiedByUser { get; set; }
	
		/// <summary>
		/// The Sponsor for this Account.
		/// </summary>
	    IAccount Sponsor { get; set; }
	
		/// <summary>
		/// The User for this Account.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountContactTags for this Account.
		/// </summary>
		IEnumerable<IAccountContactTag> AccountContactTags { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountContactTag"/> to the AccountContactTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to add.</param>
		void AddAccountContactTag(IAccountContactTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountContactTag"/> from the AccountContactTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to remove.</param>
		void RemoveAccountContactTag(IAccountContactTag item);
	
		/// <summary>
		/// The AccountLanguages for this Account.
		/// </summary>
		IEnumerable<IAccountLanguage> AccountLanguages { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountLanguage"/> to the AccountLanguages collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLanguage"/> to add.</param>
		void AddAccountLanguage(IAccountLanguage item);
	
		/// <summary>
		/// Removes an <see cref="IAccountLanguage"/> from the AccountLanguages collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountLanguage"/> to remove.</param>
		void RemoveAccountLanguage(IAccountLanguage item);
	
		/// <summary>
		/// The AccountListValues for this Account.
		/// </summary>
		IEnumerable<IAccountListValue> AccountListValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountListValue"/> to the AccountListValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountListValue"/> to add.</param>
		void AddAccountListValue(IAccountListValue item);
	
		/// <summary>
		/// Removes an <see cref="IAccountListValue"/> from the AccountListValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountListValue"/> to remove.</param>
		void RemoveAccountListValue(IAccountListValue item);
	
		/// <summary>
		/// The AccountPaymentMethods for this Account.
		/// </summary>
		IEnumerable<IAccountPaymentMethod> AccountPaymentMethods { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPaymentMethod"/> to the AccountPaymentMethods collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPaymentMethod"/> to add.</param>
		void AddAccountPaymentMethod(IAccountPaymentMethod item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPaymentMethod"/> from the AccountPaymentMethods collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPaymentMethod"/> to remove.</param>
		void RemoveAccountPaymentMethod(IAccountPaymentMethod item);
	
		/// <summary>
		/// The AccountPhones for this Account.
		/// </summary>
		IEnumerable<IAccountPhone> AccountPhones { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPhone"/> to the AccountPhones collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPhone"/> to add.</param>
		void AddAccountPhone(IAccountPhone item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPhone"/> from the AccountPhones collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPhone"/> to remove.</param>
		void RemoveAccountPhone(IAccountPhone item);
	
		/// <summary>
		/// The AccountPolicies for this Account.
		/// </summary>
		IEnumerable<IAccountPolicy> AccountPolicies { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPolicy"/> to the AccountPolicies collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPolicy"/> to add.</param>
		void AddAccountPolicy(IAccountPolicy item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPolicy"/> from the AccountPolicies collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPolicy"/> to remove.</param>
		void RemoveAccountPolicy(IAccountPolicy item);
	
		/// <summary>
		/// The AccountProperties for this Account.
		/// </summary>
		IEnumerable<IAccountProperty> AccountProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountProperty"/> to the AccountProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountProperty"/> to add.</param>
		void AddAccountProperty(IAccountProperty item);
	
		/// <summary>
		/// Removes an <see cref="IAccountProperty"/> from the AccountProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountProperty"/> to remove.</param>
		void RemoveAccountProperty(IAccountProperty item);
	
		/// <summary>
		/// The AccountPublicContactInfos for this Account.
		/// </summary>
		IEnumerable<IAccountPublicContactInfo> AccountPublicContactInfos { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPublicContactInfo"/> to the AccountPublicContactInfos collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPublicContactInfo"/> to add.</param>
		void AddAccountPublicContactInfo(IAccountPublicContactInfo item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPublicContactInfo"/> from the AccountPublicContactInfos collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPublicContactInfo"/> to remove.</param>
		void RemoveAccountPublicContactInfo(IAccountPublicContactInfo item);
	
		/// <summary>
		/// The AccountReports for this Account.
		/// </summary>
		IEnumerable<IAccountReport> AccountReports { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountReport"/> to the AccountReports collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountReport"/> to add.</param>
		void AddAccountReport(IAccountReport item);
	
		/// <summary>
		/// Removes an <see cref="IAccountReport"/> from the AccountReports collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountReport"/> to remove.</param>
		void RemoveAccountReport(IAccountReport item);
	
		/// <summary>
		/// The Accounts1 for this Account.
		/// </summary>
		IEnumerable<IAccount> Accounts1 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccount"/> to the Accounts1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to add.</param>
		void AddAccounts1(IAccount item);
	
		/// <summary>
		/// Removes an <see cref="IAccount"/> from the Accounts1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to remove.</param>
		void RemoveAccounts1(IAccount item);
	
		/// <summary>
		/// The Accounts11 for this Account.
		/// </summary>
		IEnumerable<IAccount> Accounts11 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccount"/> to the Accounts11 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to add.</param>
		void AddAccounts11(IAccount item);
	
		/// <summary>
		/// Removes an <see cref="IAccount"/> from the Accounts11 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to remove.</param>
		void RemoveAccounts11(IAccount item);
	
		/// <summary>
		/// The AccountSponsors for this Account.
		/// </summary>
		IEnumerable<IAccountSponsor> AccountSponsors { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountSponsor"/> to the AccountSponsors collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to add.</param>
		void AddAccountSponsor(IAccountSponsor item);
	
		/// <summary>
		/// Removes an <see cref="IAccountSponsor"/> from the AccountSponsors collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to remove.</param>
		void RemoveAccountSponsor(IAccountSponsor item);
	
		/// <summary>
		/// The AccountSponsors1 for this Account.
		/// </summary>
		IEnumerable<IAccountSponsor> AccountSponsors1 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountSponsor"/> to the AccountSponsors1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to add.</param>
		void AddAccountSponsors1(IAccountSponsor item);
	
		/// <summary>
		/// Removes an <see cref="IAccountSponsor"/> from the AccountSponsors1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to remove.</param>
		void RemoveAccountSponsors1(IAccountSponsor item);
	
		/// <summary>
		/// The AccountTags for this Account.
		/// </summary>
		IEnumerable<IAccountTag> AccountTags { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTag"/> to the AccountTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to add.</param>
		void AddAccountTag(IAccountTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTag"/> from the AccountTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to remove.</param>
		void RemoveAccountTag(IAccountTag item);
	
		/// <summary>
		/// The AccountTags1 for this Account.
		/// </summary>
		IEnumerable<IAccountTag> AccountTags1 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTag"/> to the AccountTags1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to add.</param>
		void AddAccountTags1(IAccountTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTag"/> from the AccountTags1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to remove.</param>
		void RemoveAccountTags1(IAccountTag item);
	
		/// <summary>
		/// The Addresses for this Account.
		/// </summary>
		IEnumerable<IAddress> Addresses { get; }
	
		/// <summary>
		/// Adds an <see cref="IAddress"/> to the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to add.</param>
		void AddAddress(IAddress item);
	
		/// <summary>
		/// Removes an <see cref="IAddress"/> from the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to remove.</param>
		void RemoveAddress(IAddress item);
	
		/// <summary>
		/// The EmailSignatures for this Account.
		/// </summary>
		IEnumerable<IEmailSignature> EmailSignatures { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailSignature"/> to the EmailSignatures collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailSignature"/> to add.</param>
		void AddEmailSignature(IEmailSignature item);
	
		/// <summary>
		/// Removes an <see cref="IEmailSignature"/> from the EmailSignatures collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailSignature"/> to remove.</param>
		void RemoveEmailSignature(IEmailSignature item);
	
		/// <summary>
		/// The Notes for this Account.
		/// </summary>
		IEnumerable<INote> Notes { get; }
	
		/// <summary>
		/// Adds an <see cref="INote"/> to the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to add.</param>
		void AddNote(INote item);
	
		/// <summary>
		/// Removes an <see cref="INote"/> from the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to remove.</param>
		void RemoveNote(INote item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccount))]
		internal abstract class AccountContracts : IAccount
		{
		    #region Primitive properties
		
			int IAccount.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.AccountNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccount.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccount.AccountStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.MiddleName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.BirthdayUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.SponsorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.EnrollerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.EnrollmentDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IAccount.IsTaxExempt
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.TaxNumber
			{
				get { throw new NotImplementedException(); }
			}
		
			bool IAccount.IsEntity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.LastRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccount.ReceivedApplication
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.NickName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.CoApplicant
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.EntityName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IAccount.GenderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAccount.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAccount.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccount.DefaultLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IAccount.AccountStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccount.PreferedContactMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.NextRenewalUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccount.IsTaxExemptVerified
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IAccount.AccountSourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.DateLastModifiedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccount.TerminatedDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccount.TaxGeocode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccount.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			bool IAccount.IsOptedOut
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccount.Account2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountListValue IAccount.AccountListValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountSource IAccount.AccountSource
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountStatus IAccount.AccountStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountStatusChangeReason IAccount.AccountStatusChangeReason
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountType IAccount.AccountType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccount.CreatedByUser
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IGender IAccount.Gender
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccount.ModifiedByUser
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccount IAccount.Sponsor
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccount.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountContactTag> IAccount.AccountContactTags
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountContactTag(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountContactTag(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountLanguage> IAccount.AccountLanguages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountLanguage(IAccountLanguage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountLanguage(IAccountLanguage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountListValue> IAccount.AccountListValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountListValue(IAccountListValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountListValue(IAccountListValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountPaymentMethod> IAccount.AccountPaymentMethods
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountPaymentMethod(IAccountPaymentMethod item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountPaymentMethod(IAccountPaymentMethod item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountPhone> IAccount.AccountPhones
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountPhone(IAccountPhone item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountPhone(IAccountPhone item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountPolicy> IAccount.AccountPolicies
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountPolicy(IAccountPolicy item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountPolicy(IAccountPolicy item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountProperty> IAccount.AccountProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountProperty(IAccountProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountPublicContactInfo> IAccount.AccountPublicContactInfos
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountPublicContactInfo(IAccountPublicContactInfo item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountPublicContactInfo(IAccountPublicContactInfo item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountReport> IAccount.AccountReports
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountReport(IAccountReport item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountReport(IAccountReport item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccount> IAccount.Accounts1
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccounts1(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccounts1(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccount> IAccount.Accounts11
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccounts11(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccounts11(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountSponsor> IAccount.AccountSponsors
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountSponsor(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountSponsor(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountSponsor> IAccount.AccountSponsors1
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountSponsors1(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountSponsors1(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTag> IAccount.AccountTags
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountTag(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountTag(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTag> IAccount.AccountTags1
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAccountTags1(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAccountTags1(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAddress> IAccount.Addresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IEmailSignature> IAccount.EmailSignatures
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddEmailSignature(IEmailSignature item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveEmailSignature(IEmailSignature item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<INote> IAccount.Notes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccount.AddNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccount.RemoveNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
