using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Users.Common;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.Users.Common.Models;
using NetSteps.Encore.Core;

namespace NetSteps.Web.API.Users.Common
{
	/// <summary>
	/// Authenticate Users
	/// </summary>
    public class UsersController : BaseController
    {

        #region Declarations

		readonly static ICopier<Credentials, ICredentials> _copier = Create.New<ICopier<Credentials, ICredentials>>();
				
        private IUsers usersModule;

        private ILogResolver logResolver;

        private ITermResolver termResolver;

        #endregion

        #region Constructor(s)
		/// <summary>
		/// Create an instance
		/// </summary>
        public UsersController() : this(Create.New<IUsers>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())
        {
            Contract.Ensures(usersModule != null);
            Contract.Ensures(termResolver != null);
            Contract.Ensures(logResolver != null);
        }
		/// <summary>
		/// Create an instance
		/// </summary>
		/// <param name="uModule">Users Module</param>
		/// <param name="lResolver">Log Resolver</param>
		/// <param name="tResolver">Term Resolve</param>
        public UsersController(IUsers uModule, ILogResolver lResolver, ITermResolver tResolver)
        {
            this.usersModule = uModule ?? Create.New<IUsers>();
            this.logResolver = lResolver ?? Create.New<ILogResolver>();
            this.termResolver = tResolver ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Methods
       
        private bool ValidatePassword(string password, ref string message)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(password))
            {
                isValid = false;
                string term = termResolver.Term("User_Invalid_Password", "Invalid Password:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, password)
                    : string.Format("{0} {1} {2}", message, term, password);
            }

            return isValid;
        }

        private bool ValidateUsername(string username, string credentialUsername, ref string message)
        {
            bool isValid = true;            

            if (string.IsNullOrEmpty(username))
            {
                isValid = false;
                string term = termResolver.Term("User_Invalid_Username", "Invalid Username:");
                message = string.Format("{0} {1}", term, username);
            }

            if (string.IsNullOrEmpty(credentialUsername) || !username.Equals(credentialUsername))
            {
                isValid = false;
                message = termResolver.Term("User_Username_dont_match", "Username in the URL does not match the Username in Credentials");
                return isValid;
            }

            return isValid;
        }

		/// <summary>
		/// Authenticate a User
		/// 
		/// eg. http://yourdomain.com/users/authenticate/{username}
		/// </summary>
		/// <param name="username">UserName of the user to authenticate</param>
		/// <param name="credentials">Credentials of the user to authenticate</param>
		/// <seealso cref="Credentials"/>
		/// <returns>ActionResult</returns>
		/// <seealso cref="ActionResult"/>
        [HttpGet]
        [ApiAccessKeyFilter]
        public ActionResult AuthenticateUser(string username, Credentials credentials)
        {
            try
            {
                string message = string.Empty;

				var dto = Create.New<ICredentials>();
                _copier.CopyTo(dto, credentials, CopyKind.Loose, Container.Current);

                bool isValidUsername = ValidateUsername(username, credentials.Username, ref message);

                bool isValidPassword = ValidatePassword(credentials.Password, ref message);

                if (isValidUsername && isValidPassword)
                {
                    IUsersResult result = usersModule.AuthenticateUser(credentials.Username, credentials.Password);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
