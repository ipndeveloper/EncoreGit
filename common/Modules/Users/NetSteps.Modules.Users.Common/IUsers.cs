using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Users.Common
{
	/// <summary>
	/// Users
	/// </summary>
	public interface IUsers
	{
		/// <summary>
		/// Authenticate a User
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
        IUsersResult AuthenticateUser(string username, string password);
	}
}
