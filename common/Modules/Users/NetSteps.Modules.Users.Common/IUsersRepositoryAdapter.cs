using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Users.Common
{
	/// <summary>
	/// User Adapter
	/// </summary>
	public interface IUsersRepositoryAdapter
	{
		/// <summary>
		/// Athenticate a user
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		IUsersSearch AuthenticateUser(string username, string password);
	}
}
