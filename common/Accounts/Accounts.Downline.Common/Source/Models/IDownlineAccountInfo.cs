using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Extended data related to an account and their upline/downline.
	/// </summary>
	[DTO]
	public interface IDownlineAccountInfo
	{
		/// <summary>
		/// The account ID.
		/// </summary>
		int AccountId { get; set; }

		/// <summary>
		/// The tree level relative to the specified root account.
		/// </summary>
		int RelativeTreeLevel { get; set; }

		/// <summary>
		/// The number of accounts below this one in the downline.
		/// </summary>
		int DownlineCount { get; set; }

		/// <summary>
		/// The account number.
		/// </summary>
		string AccountNumber { get; set; }

		/// <summary>
		/// The account's first name.
		/// </summary>
		string FirstName { get; set; }

		/// <summary>
		/// The account's last name.
		/// </summary>
		string LastName { get; set; }

		/// <summary>
		/// The sponsor's account number.
		/// </summary>
		string SponsorAccountNumber { get; set; }

		/// <summary>
		/// The sponsor's first name.
		/// </summary>
		string SponsorFirstName { get; set; }

		/// <summary>
		/// The sponsor's last name.
		/// </summary>
		string SponsorLastName { get; set; }

		/// <summary>
		/// The enroller's account number.
		/// </summary>
		string EnrollerAccountNumber { get; set; }

		/// <summary>
		/// The enroller's first name.
		/// </summary>
		string EnrollerFirstName { get; set; }

		/// <summary>
		/// The enroller's last name.
		/// </summary>
		string EnrollerLastName { get; set; }

		/// <summary>
		/// The account's enrollment date.
		/// </summary>
		DateTime? EnrollmentDateUtc { get; set; }

		/// <summary>
		/// The account's next renewal date.
		/// </summary>
		DateTime? NextRenewalDateUtc { get; set; }

		/// <summary>
		/// The account type ID.
		/// </summary>
		short AccountTypeId { get; set; }

		/// <summary>
		/// The account status ID.
		/// </summary>
		short AccountStatusId { get; set; }
		
		/// <summary>
		/// The account's email address.
		/// </summary>
		string EmailAddress { get; set; }

		/// <summary>
		/// The account's phone number.
		/// </summary>
		string PhoneNumber { get; set; }

		/// <summary>
		/// The account's PWS URL.
		/// </summary>
		string PwsUrl { get; set; }

		/// <summary>
		/// The account's address.
		/// </summary>
		string Address1 { get; set; }

		/// <summary>
		/// The account's city.
		/// </summary>
		string City { get; set; }

		/// <summary>
		/// The account's state abbreviation.
		/// </summary>
		string StateAbbreviation { get; set; }
		
		/// <summary>
		/// The account's postal code.
		/// </summary>
		string PostalCode { get; set; }

		/// <summary>
		/// The account's country ID.
		/// </summary>
		int? CountryId { get; set; }
		
		/// <summary>
		/// The commission date of the account's last completed order.
		/// </summary>
		DateTime? LastOrderCommissionDateUtc { get; set; }

		/// <summary>
		/// The date of the account's next autoship run.
		/// </summary>
		DateTime? NextAutoshipRunDate { get; set; }
	}
}
