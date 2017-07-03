using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Basic data related to an account and their parent in the hierarchy.
	/// </summary>
	[DTO]
	public interface IDownlineData
	{
		/// <summary>
		/// The account ID.
		/// </summary>
		int AccountId { get; set; }
		
		/// <summary>
		/// The parent account ID in the hierarchy.
		/// </summary>
		int? ParentAccountId { get; set; }

		/// <summary>
		/// The account's absolute tree level.
		/// </summary>
		int TreeLevel { get; set; }

		/// <summary>
		/// The account type ID.
		/// </summary>
		short AccountTypeId { get; set; }

		/// <summary>
		/// The account status ID.
		/// </summary>
		short AccountStatusId { get; set; }

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
	}
}
