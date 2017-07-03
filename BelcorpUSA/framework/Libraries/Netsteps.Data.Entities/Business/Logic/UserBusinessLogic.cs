using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class UserBusinessLogic
	{
		/// <summary>
		/// Check to see if the password a user wanted to change to is inline with the clients password policy. - JHE
		/// </summary>
		/// <param name="entity"></param>
		public BasicResponse NewPasswordIsValid(string newPassword, string newPasswordConfirmation)
		{
			try
			{
				BasicResponse response = new BasicResponse();
				response.Success = true;
				if (string.IsNullOrWhiteSpace(newPassword))
				{
					response.Message = "Please enter a password.";
					response.Success = false;
					return response;
				}
				if (newPassword != newPasswordConfirmation)
				{
					response.Message = "Passwords do not match.";
					response.Success = false;
					return response;
				}
				if (newPassword.Length < 6)
				{
					response.Message = "Password must be 6 characters or longer.";
					response.Success = false;
					return response;
				}
				if (newPassword == newPassword.GetIncrementalNumberStringEquivalent())
				{
					response.Message = "Invalid password. Please choose a different password.";
					response.Success = false;
					return response;
				}

				return response;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void LogFailedLogin(User entity)
		{
			try
			{
				entity.StartEntityTracking();
				entity.ConsecutiveFailedLogins = entity.ConsecutiveFailedLogins + 1;

				// Lock the account out if it goes over a specified TotalFailedLoginAttempts. - JHE
				if (entity.ConsecutiveFailedLogins > 10)
					entity.UserStatusID = Constants.UserStatus.LockedOut.ToShort();

				entity.Save();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void LogSucessfulLogin(User entity)
		{
			try
			{
				using (new NetSteps.Common.OperationDebugTimer("LogSucessfulLogin"))
				{
					entity.StopEntityTracking();
					entity.RemoveEntitiesFromObjectGraph<Account>();
					entity.StartEntityTracking();
					entity.ConsecutiveFailedLogins = 0; // Reset Failed attempts on successful attempt. - JHE
					entity.LastLogin = DateTime.Now;
					entity.TotalLoginCount = entity.TotalLoginCount + 1;

					entity.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public string GeneratePassword()
		{
			StringBuilder builder = new StringBuilder();
			Random rand = new Random();
			for (var i = 0; i < 8; i++)
			{
				builder.Append((char)rand.Next(48, 124));
			}
			return builder.ToString();
		}

		public override void Delete(Repositories.IUserRepository repository, User entity)
		{
			if (entity.Roles != null)
			{
				foreach (var role in entity.Roles)
				{
					if (role.ChangeTracker.State != ObjectState.Added)
						role.MarkAsDeleted();
				}
			}

			base.Delete(repository, entity);
		}

		public virtual bool IsUsernameAvailable(Repositories.IUserRepository repository, int userID, string username)
		{
			try
			{
				return repository.IsUsernameAvailable(userID, username);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual User Authenticate(Repositories.IUserRepository repository, string username, string password)
		{
			try
			{
				var user = repository.Authenticate(username, password);
				if (user != null)
				{
					user.StartTracking();
					user.IsLazyLoadingEnabled = true;
				}
				return user;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual UserSlimSearchData LoadSlim(IUserRepository repository, int userID)
		{
			try
			{
				return repository.LoadSlim(userID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<UserSiteWidget> LoadUserSiteWigets(Repositories.IUserRepository repository, int userID)
		{
			try
			{
				var userSiteWidgets = repository.LoadUserSiteWigets(userID);
				foreach (var userSiteWidget in userSiteWidgets)
				{
					userSiteWidget.StartEntityTracking();
					userSiteWidget.IsLazyLoadingEnabled = true;
				}
				return userSiteWidgets;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Dictionary<int, string> SlimSearch(IUserRepository repository, string query, int? userTypeID = null, int? userStatusID = null)
		{
			try
			{
				userTypeID = userTypeID.HasValue ? userTypeID : (int)Constants.UserType.Corporate;
				userStatusID = userStatusID.HasValue ? userStatusID : (int)Constants.UserStatus.Active;

				return repository.SlimSearch(query, userTypeID, userStatusID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual User GetByUserName(IUserRepository repository, string username)
		{
			try
			{
				return repository.GetByUsername(username);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual bool SendResetPasswordEmail(string username, int siteID)
		{
			var retVal = false;
			try
			{
				var site = Site.LoadSiteWithSiteURLs(siteID);
				var primary = site.SiteUrls.FirstOrDefault(x => x.IsPrimaryUrl);
				var siteUrl = primary != null ? primary.Url : string.Empty;
                siteUrl = siteUrl.Contains("https://") ? siteUrl : siteUrl.Replace("http://", "https://");
				var user = User.GetByUsername(username);
				if (user == null || user.UserStatusID != (short)Constants.UserStatus.Active)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(Translation.GetTerm("UserInactiveOrNotFound", "User inactive or not found"), Constants.NetStepsExceptionType.NetStepsBusinessException);
				}
				var account = Account.GetByUserId(user.UserID);
				if (account == null || !(account.AccountStatusID == (short)Constants.AccountStatus.Active || account.AccountStatusID == (short)Constants.AccountStatus.Imported))
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(Translation.GetTerm("ProblemLoadingAccount", "Problem Loading Account"), Constants.NetStepsExceptionType.NetStepsBusinessException);
				}
				var emailAddress = account.EmailAddress;
				var name = account.FirstName + ' ' + account.LastName;
				var accountStr = Account.GetSingleSignOnToken(account.AccountID);
				var url = string.Format("{0}{1}", !string.IsNullOrEmpty(siteUrl) ? siteUrl : "http://www" + GetDistributorDomain(), "ResetPassword?accountStr=" + accountStr).ConvertToSecureUrl(ConfigurationManager.ForceSSL);
				var corporateMailAccount = MailAccount.LoadFull(ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateMailAccountID));

				var passwordResetEmailTemplate = EmailTemplate.Search(new EmailTemplateSearchParameters()
				{
					Active = true,
					PageIndex = 0,
					PageSize = 1,
					EmailTemplateTypeIDs =
					new List<short>
					{
						(short)Constants.EmailTemplateType.ForgotPassword
					}
				}).FirstOrDefault();

				if (passwordResetEmailTemplate != null)
				{
					var translation = passwordResetEmailTemplate.EmailTemplateTranslations.GetByLanguageID(account.DefaultLanguageID);
					if (translation != null)
					{
						var message = translation.GetTokenReplacedMailMessage(new ForgotPasswordTokenValueProvider(url.EldEncode(), name));
						message.To.Add(new MailMessageRecipient(emailAddress));
						message.Send(corporateMailAccount, siteID);
						retVal = true;
					}
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			return retVal;
		}

		protected virtual string GetDistributorDomain()
		{
			string domains = ConfigurationManager.AppSettings["Domains"];

			if (string.IsNullOrWhiteSpace(domains))
			{
				return string.Empty;
			}

			return domains.Split(';').FirstOrDefault();
		}
	}
}
