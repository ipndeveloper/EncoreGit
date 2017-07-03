using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Security;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class UserRepository : IUserRepository
	{
		#region Members
		protected override Func<NetStepsEntities, int, IQueryable<User>> loadFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<User>>(
				 (context, userId) => from u in context.Users
								.Include("Roles")
								.Include("Roles.Functions")
								.Include("UserFunctionOverrides")
								.Include("UserFunctionOverrides.Function")
									  where u.UserID == userId
									  select u);
			}
		}

		protected override Func<NetStepsEntities, IQueryable<User>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<User>>(
				 (context) => from u in context.Users
								.Include("Roles")
								.Include("Roles.Functions")
								.Include("UserFunctionOverrides")
								.Include("UserFunctionOverrides.Function")
							  select u);
			}
		}
		#endregion

		#region Methods
		public User Authenticate(string username, string password)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var user = context.Users.Include("Roles")
												.Include("Roles.Functions")
												.Include("UserFunctionOverrides")
												.Include("UserFunctionOverrides.Function")
								.FirstOrDefault(u => u.Username == username);

					if (user == null)
						throw new NetStepsBusinessException("Username does not exist.")
						{
							PublicMessage = Translation.GetTerm("InvalidCredentials", "Invalid credentials."),
							IncludeErrorLogMessage = false
						};

					if (user.UserStatusID != Constants.UserStatus.Active.ToInt())
					{
						user.LogFailedLogin();
						throw new NetStepsBusinessException("User is inactive or locked out.")
						{
							PublicMessage = Translation.GetTerm("UserIsInactiveOrLockedOut", "User is inactive or locked out."),
							IncludeErrorLogMessage = false
						};
					}

					if (SimpleHash.VerifyHash(password, SimpleHash.Algorithm.SHA512, user.PasswordHash))
					{
						user.LogSucessfulLogin();
						return user;
					}
					else
					{
						user.LogFailedLogin();
						throw new NetStepsBusinessException("Invalid credentials.")
						{
							PublicMessage = Translation.GetTerm("InvalidCredentials", "Invalid credentials."),
							IncludeErrorLogMessage = false
						};
					}
				}
			});
		}

		/// <summary>
		/// UserID is the userID of the person requesting the name. Pass in 0 for a new person. - JHE 
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool IsUsernameAvailable(int userID, string username)
		{
			if (string.IsNullOrEmpty(username.ToCleanString()))
				return false;

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var user = context.Users.FirstOrDefault(u => u.Username == username);

					return user == null || user.UserID == userID;
				}
			});
		}

		public UserSlimSearchData LoadSlim(int userID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var result = context.Users.Where(u => u.UserID == userID).Select(u => new
									   {
										   u.UserID,
										   u.UserTypeID,
										   u.Username
									   }).FirstOrDefault();

					if (result == null)
						throw new NetStepsDataException("Error loading User. Invalid userID: " + userID);
					else
						return new UserSlimSearchData()
						{
							UserID = result.UserID,
							UserTypeID = result.UserTypeID,
							Username = result.Username
						};
				}
			});
		}

		public List<UserSiteWidget> LoadUserSiteWigets(int userID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.UserSiteWidgets.Where(usw => usw.UserID == userID).ToList();
				}
			});
		}

		/// <summary>
		/// Returns the account id as the key and first name, last name, and account number as the value
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public Dictionary<int, string> SlimSearch(string query, int? userTypeID = null, int? userStatusID = null)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Users.Where(a =>
						   (!userStatusID.HasValue || a.UserStatusID == userStatusID.Value)
						&& (!userTypeID.HasValue || a.UserTypeID == userTypeID.Value)
						&& (a.Username.Contains(query)))
						.Select(p => new
						{
							p.UserID,
							p.Username
						})
						.ToDictionary(a => a.UserID, a => a.Username);
				}
			});
		}

		public User GetByUsername(string username)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var user = context.Users.FirstOrDefault(u => u.Username == username && u.UserStatusID == (int)Constants.UserStatus.Active);
					return user;
				}
			});
		}

		protected override string GetMeaningfulAuditValue(string tableName, string columnName, string value)
		{
			try
			{
				if (columnName == "PasswordHash")
					return string.Format("({0})", Translation.GetTerm("encrypted"));
				else
					return base.GetMeaningfulAuditValue(tableName, columnName, value);
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
				return value;
			}
		}
		#endregion
	}
}