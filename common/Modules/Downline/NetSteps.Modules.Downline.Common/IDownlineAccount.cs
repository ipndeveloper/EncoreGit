using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Downline.Common
{
	/// <summary>
	/// Downline Account
	/// </summary>
	[DTO]
	public interface IDownlineAccount
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// FirstName
		/// </summary>
		string FirstName { get; set; }
		/// <summary>
		/// LastName
		/// </summary>
		string LastName { get; set; }
		/// <summary>
		/// Account Type
		/// </summary>
		int AccountTypeID { get; set; }
		/// <summary>
		/// Enrollment Date
		/// </summary>
		DateTime EnrollmentDate { get; set; }
		/// <summary>
		/// Account Status
		/// </summary>
		int AccountStatus { get; set; }
	}
}
