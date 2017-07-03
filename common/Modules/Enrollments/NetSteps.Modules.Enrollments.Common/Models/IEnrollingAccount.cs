using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrolling Account
	/// </summary>
	[DTO]
	public interface IEnrollingAccount
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// AccountTypeID
		/// </summary>
		short AccountTypeID { get; set; }
		/// <summary>
		/// SponsorID
		/// </summary>
		int SponsorID { get; set; }
		/// <summary>
		/// Placement
		/// </summary>
		int Placement { get; set; }
		/// <summary>
		/// First Name
		/// </summary>
		string FirstName { get; set; }
		/// <summary>
		/// Middle Name
		/// </summary>
		string MiddleName { get; set; }
		/// <summary>
		/// Last Name
		/// </summary>
		string LastName { get; set; }
		/// <summary>
		/// Email
		/// </summary>
		string Email { get; set; }
		/// <summary>
		/// Tax Number
		/// </summary>
		string TaxNumber { get; set; }
		/// <summary>
		/// Tax Exempt
		/// </summary>
		bool TaxExempt { get; set; }
		/// <summary>
		/// Is the account an Entity
		/// </summary>
		bool IsEntity { get; set; }
		/// <summary>
		/// Date Of Birth
		/// </summary>
		DateTime DateOfBirth { get; set; }
		/// <summary>
		/// LanguageID
		/// </summary>
		int LanguageID { get; set; }
		/// <summary>
		/// GenderID
		/// </summary>
		short GenderID { get; set; }
		/// <summary>
		/// Phone Number
		/// </summary>
		string PhoneNumber { get; set; }
		/// <summary>
		/// Main Address
		/// </summary>
		IEnrollmentAddress MainAddress { get; set; }
		/// <summary>
		/// Shipping Address
		/// </summary>
		IEnrollmentAddress ShippingAddress { get; set; }
		/// <summary>
		/// Billing Profile
		/// </summary>
		IEnrollmentBillingProfile BillingProfile { get; set; }
		//string Password { get; set; }
		//int ShippingMethodID { get; set; }
	}
}
