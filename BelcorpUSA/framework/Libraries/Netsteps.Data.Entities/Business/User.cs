using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Authorization.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{
	public partial class User
	{
		#region Members

		private List<Function> _functions;

		#endregion

		#region Properties

        public bool PasswordIsIdentical { get; set; }

		public string Password
		{
			set
			{
				if (!value.IsNullOrEmpty())
				{
					PasswordHash = SimpleHash.ComputeHash(value, SimpleHash.Algorithm.SHA512, null);
				}
			}
		}

		public List<Function> Functions
		{
			get
			{
				if (_functions == null)
				{
					_functions = new List<Function>();
					var comparer = new LambdaComparer<Function>((x, y) => x.FunctionID == y.FunctionID);
					var authorizationService = Create.New<IAuthorizationService>();
					foreach (var role in Roles)
					{
						// authorization service call will filter functions before they are added to the user's set of functions.
						foreach (var eachFunction in role.Functions)
						{
							if (!_functions.Contains(eachFunction, comparer) && authorizationService.FilterAuthorizationFunctions(new string[] { eachFunction.Name }).Any())
							{
								_functions.Add(eachFunction);
							}
						}
					}
				}
				return _functions;
			}
		}

		#endregion

		#region Methods
		public bool IsUsernameAvailable(string username)
		{
			try
			{
				return BusinessLogic.IsUsernameAvailable(Repository, UserID, username);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		/// <summary>
		/// UserID is the userID of the person requesting the name. Pass in 0 for a new person. - JHE 
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public static bool IsUsernameAvailable(int userID, string username)
		{
			try
			{
				return BusinessLogic.IsUsernameAvailable(Repository, userID, username);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		internal static User Authenticate(string username, string password, int? accountTypeID = null)
		{
			try
			{
				return BusinessLogic.Authenticate(Repository, username, password);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public bool HasFunction(string title, bool checkAnonymousRole = true, bool checkWorkstationUserRole = true, int? accountTypeID = null)
		{
			try
			{
				// this allows the authorization service to determine if a user has access to the selected function.
				// Logic for authorization should be moved (eventually) to that code domain.
				var authorizationService = Create.New<IAuthorizationService>();
				var nonBlockedFunctions = authorizationService.FilterAuthorizationFunctions(new string[] { title });
				if (!nonBlockedFunctions.Any())
				{
					return false;
				}

				var function = Functions.GetByName(title);
				var overrideFunction = UserFunctionOverrides.FirstOrDefault(f => f.Function.Name == title);

				Role anonymousRole = ApplicationContext.Instance.AnonymousRole;
				Role workstationUserRole = ApplicationContext.Instance.WorkstationUserRole;

				// We look at the UserFunctionOverrides a user may have to determine if they should have access. - JHE
				if (function == null && overrideFunction == null)
				{
					Function.CreateFunction(title);
				}
				else
				{
					if (overrideFunction != null)
					{
						return overrideFunction.Allow;
					}

					return function.Active;
				}

				if (checkAnonymousRole && anonymousRole != null && anonymousRole.HasFunction(title))
				{
					return true;
				}
				if (checkWorkstationUserRole && workstationUserRole != null && workstationUserRole.HasFunction(title))
				{
					return true;
				}

				if (accountTypeID.HasValue)
				{
					var accountType = SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID.ToShort());

					foreach (var role in accountType.Roles)
					{
						if (role.HasFunction(title))
						{
							return true;
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public bool HasRole(string roleName)
		{
			try
			{
				var role = Roles.GetByName(roleName);

				// We look at the UserFunctionOverrides a user may have to determine if they should have access. - JHE
				if (role == null)
				{
					Role.CreateRole(roleName);
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static BasicResponse NewPasswordIsValid(string newPassword, string newPasswordConfirmation)
		{
			try
			{
				return BusinessLogic.NewPasswordIsValid(newPassword, newPasswordConfirmation);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void LogFailedLogin()
		{
			try
			{
				BusinessLogic.LogFailedLogin(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void LogSucessfulLogin()
		{
			try
			{
				BusinessLogic.LogSucessfulLogin(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static string GeneratePassword()
		{
			try
			{
				return BusinessLogic.GeneratePassword();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static UserSlimSearchData LoadSlim(int userID)
		{
			try
			{
				return BusinessLogic.LoadSlim(Repository, userID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<UserSiteWidget> LoadUserSiteWigets(int userID)
		{
			try
			{
				return BusinessLogic.LoadUserSiteWigets(Repository, userID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Returns the account id as the key and first name, last name, and account number as the value
		/// </summary>
		/// <param name="query"></param>
		/// <param name="userTypeID"></param>
		/// <param name="userStatusID"></param>
		/// <returns></returns>
		public static Dictionary<int, string> SlimSearch(string query, int? userTypeID = null, int? userStatusID = null)
		{
			try
			{
				return BusinessLogic.SlimSearch(Repository, query, userTypeID, userStatusID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Returns the first result of a SlimSearch
		/// </summary>
		/// <param name="query"></param>
		/// <param name="userTypeID"></param>
		/// <param name="userStatusID"></param>
		/// <returns></returns>
		public static KeyValuePair<int, string> SlimSearchGetFirstResult(string query, int? userTypeID = null, int? userStatusID = null)
		{
			try
			{
				var results = BusinessLogic.SlimSearch(Repository, query, userTypeID, userStatusID);
				var firstResult = results != null && results.Any() ? results.First() : new KeyValuePair<int, string>();

				return firstResult;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static User GetByUsername(string username)
		{
			try
			{
				return BusinessLogic.GetByUserName(Repository, username);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool SendResetPasswordEmail(string username, int siteID)
		{
			try
			{
				return BusinessLogic.SendResetPasswordEmail(username, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#endregion

       
    }
}