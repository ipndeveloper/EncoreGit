using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Users.Common
{
	/// <summary>
	/// User Search
	/// </summary>
	[DTO]
	public interface IUsersSearch
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
        /// <summary>
		/// Username
        /// </summary>
        string Username { get; set; }
		/// <summary>
		/// Password
		/// </summary>
        string Password { get; set; }
		/// <summary>
		/// Account Properties
		/// </summary>
		Dictionary<string, string> AccountProperties { get; set; }
	}
}
