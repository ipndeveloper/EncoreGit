using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Users.Common;

namespace NetSteps.Modules.Users.Common
{
	/// <summary>
	/// Default Implementation of IUsers
	/// </summary>
	[ContainerRegister(typeof(IUsers), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultUsers : IUsers
    {

        #region Declarations

        private IUsersRepositoryAdapter userRepository;

        #endregion

        #region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
        public DefaultUsers() : this(Create.New<IUsersRepositoryAdapter>())
		{ 
		}
		/// <summary>
		/// Constructor with an adapter
		/// </summary>
		/// <param name="userRepo"></param>
		public DefaultUsers(IUsersRepositoryAdapter userRepo)
		{
            userRepository = userRepo ?? Create.New<IUsersRepositoryAdapter>();
		}

        #endregion

        #region Methods

		/// <summary>
		/// Authenticate a user
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
        public IUsersResult AuthenticateUser(string username, string password)
		{
            Contract.Requires<ArgumentException>(username != null || password != null);
			var result = Create.New<IUsersResult>();

			result.Success = false;
            var site = userRepository.AuthenticateUser(username, password);

			if (site != null)
			{               
				result.AccountID = site.AccountID;				
                result.Username = site.Username;
				result.AccountProperties = site.AccountProperties;
				result.Success = true;
			}

			return result;
        }

        #endregion

    }
}
