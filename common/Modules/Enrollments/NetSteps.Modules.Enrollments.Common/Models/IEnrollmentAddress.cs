using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Address
	/// </summary>
	[DTO]
	public interface IEnrollmentAddress
	{
		/// <summary>
		/// Attention
		/// </summary>
		string Attention { get; set; }
		/// <summary>
		/// AddressLine1
		/// </summary>
		string AddressLine1 { get; set; }
		/// <summary>
		/// AddressLine2
		/// </summary>
		string AddressLine2 { get; set; }
		/// <summary>
		/// AddressLine3
		/// </summary>
		string AddressLine3 { get; set; }
		/// <summary>
		/// PostalCode
		/// </summary>
		string PostalCode { get; set; }
		/// <summary>
		/// City
		/// </summary>
		string City { get; set; }
		/// <summary>
		/// County
		/// </summary>
		string County { get; set; }
		/// <summary>
		/// State
		/// </summary>
		string State { get; set; }
		/// <summary>
		/// CountryID
		/// </summary>
		int CountryID { get; set; }
	}
}
