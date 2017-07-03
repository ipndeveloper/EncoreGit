using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Web.API.Users.Common.Models
{
	/// <summary>
	/// Credentials
	/// </summary>
	public class Credentials
	{
		/// <summary>
		/// Username
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; }
	}

	/// <summary>
	/// Credentials
	/// </summary>
	[DTO]
	public interface ICredentials
	{
		/// <summary>
		/// Username
		/// </summary>
        string Username { get; set; }
		/// <summary>
		/// Password
		/// </summary>
		string Password { get; set; }
	}
}
