using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Billing Profile
	/// </summary>
	[DTO]
	public interface IEnrollmentBillingProfile
	{
		/// <summary>
		/// NameOnCard
		/// </summary>
		string NameOnCard { get; set; }
		/// <summary>
		/// PaymentTypeID
		/// </summary>
		int PaymentTypeID { get; set; }
		/// <summary>
		/// Credit Card Number
		/// </summary>
		string CCNumber { get; set; }
		/// <summary>
		/// Expiration Date
		/// </summary>
		DateTime ExpirationDate { get; set; }
		/// <summary>
		/// CVV
		/// </summary>
		string CVV { get; set; }
		/// <summary>
		/// Billing Address
		/// </summary>
		IEnrollmentAddress BillingAddress { get; set; }
	}
}
