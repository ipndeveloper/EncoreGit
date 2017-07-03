using System;
using System.Diagnostics;
using System.Linq;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Configuration;
using NetSteps.Auth.Common.Model;
using NetSteps.Auth.UI.Common;
using NetSteps.Auth.UI.Common.DataObjects;
using NetSteps.Auth.UI.Common.Enumerations;
using NetSteps.Common.Interfaces;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.UI.Service.Services
{
	[ContainerRegister(typeof(IAuthenticationUIService), RegistrationBehaviors.Default)]
	public class AuthenticationUIService : IAuthenticationUIService
	{
		static TraceSource ts = new TraceSource("traceSource"); 
		
		private IAuthenticationService _authService;
		private ITermTranslationProvider _translation;

		public AuthenticationUIService() : this(Create.New<IAuthenticationService>(), Create.New<ITermTranslationProvider>()) { }

		public AuthenticationUIService(IAuthenticationService authService, ITermTranslationProvider translation)
		{
			this.TraceInformation(string.Format("IAuthenticationService is {0}", authService.GetType()));
			this.TraceInformation(string.Format("ITermTranslationProvider is {0}", translation.GetType()));

			_authService = authService;
			_translation = translation;
		}

		/// <summary>
		/// Calls the AUth Services Authenticate() and returns the results as a <see cref="IAuthenticationUIReult"/>
		/// for the UI to display
		/// </summary>
		/// <param name="userUniqueIdentifier">the users login name</param>
		/// <param name="password">the users password</param>
		/// <param name="siteId">the ID of the site the user is logging into</param>
		/// <returns><see cref="IAuthenticationUIResult"/></returns>
		public IAuthenticationUIResult Authenticate(string userUniqueIdentifier, string password, int siteId)
		{
			using (var authenticateTrace = this.TraceActivity(string.Format("doing Authenticate for {0}", userUniqueIdentifier)))
			{
				try
				{
					ICredentials cred = new Credentials()
						{
							Password = password,
							UserUniqueIdentifier = userUniqueIdentifier,
							SiteID = siteId
						};

					IAuthenticationUIResult returnAuthResult = new AuthenticationUIResult();
					returnAuthResult.WasLoginSuccessful = false;

					IAuthenticationResult authResult = _authService.Authenticate(cred);
					AuthenticationResultType typeOfResult = (AuthenticationResultType)authResult.AuthenticationResultTypeID;

					using (var resultTypeTrace = this.TraceActivity(string.Format("checking typeOfResult for value {0}", typeOfResult)))
					{
						if (typeOfResult == AuthenticationResultType.Success)
						{
							returnAuthResult.WasLoginSuccessful = true;

							//set the return objects CredentialTypeID to the
							//type that was used to successfully login
							string successfulProviderName = authResult.ProviderResponseMessages.First(p => p.AuthenticationResultTypeID == (int)AuthenticationResultType.Success).ProviderName;
							switch (successfulProviderName)
							{
								case EncoreAuthenticationProviderNames.AccountIDProvider:
									returnAuthResult.CredentialTypeID = (int)LoginCredentialTypes.AccountId;
									break;
								case EncoreAuthenticationProviderNames.CorporateUsernameProvider:
									returnAuthResult.CredentialTypeID = (int)LoginCredentialTypes.CorporateUsername;
									break;
								case EncoreAuthenticationProviderNames.EmailAddressProvider:
									returnAuthResult.CredentialTypeID = (int)LoginCredentialTypes.Email;
									break;
								case EncoreAuthenticationProviderNames.UsernameProvider:
									returnAuthResult.CredentialTypeID = (int)LoginCredentialTypes.Username;
									break;
								default:
									//do nothing
									break;
							}
							this.TraceInformation(string.Format("set CredentialTypeID to {0}", returnAuthResult.CredentialTypeID));
						}
						else if (typeOfResult == AuthenticationResultType.InvalidUserIdentifierFormat || typeOfResult == AuthenticationResultType.InvalidPassword || typeOfResult == AuthenticationResultType.InvalidUserIdentifier)
						{
							var failMessage = _translation.GetTerm("InvalidLogin", "Invalid Login.  Please Try Again.");
							this.TraceInformation(string.Format("typeOfResult was {0} setting FailureMessage to {1}"
								, typeOfResult
								, failMessage
								));
							returnAuthResult.FailureMessage = failMessage;
						}
						else if (typeOfResult == AuthenticationResultType.NoRegisteredProviders || typeOfResult == AuthenticationResultType.ProviderException
																								|| typeOfResult == AuthenticationResultType.Untried)
						{
							var failMessage = _translation.GetTerm("Login_ProviderErrorMessage", "We could not process your login request. Please Try Again.");
							this.TraceInformation(string.Format("typeOfResult was {0} setting FailureMessage to {1}"
								, typeOfResult
								, failMessage
								));
							returnAuthResult.FailureMessage = failMessage;
						}
					}

					this.TraceInformation(string.Format("returning authResult: WasLoginSuccessful - {0}"
						, (returnAuthResult != null) ? returnAuthResult.WasLoginSuccessful.ToString() : "null"));

					return returnAuthResult;
				}
				catch (Exception excp)
				{
					excp.TraceException(excp);
					throw;
				}
			}
		}

		/// <summary>
		/// Calls the Auth Services RetrievePassword() and returns the results as a <see cref="IForgotPasswordUIResult"/>
		/// for the UI to display
		/// </summary>
		/// <param name="userUniqueIdentifier">The users login name</param>
		/// <param name="siteId">The ID of the site the user is logging into</param>
		/// <returns><see cref="IForgotPasswordUIResult"/></returns>
		public IForgotPasswordUIResult ForgotPassword(string userUniqueIdentifier, int siteId)
		{
			IPartialCredentials creds = new PartialCredentials();
			creds.UserUniqueIdentifier = userUniqueIdentifier;
			creds.SiteID = siteId;

			IPasswordRetrievalResult result = _authService.RetrieveAccount(creds);
			IForgotPasswordUIResult returnResult = Create.New<IForgotPasswordUIResult>();
			returnResult.Message = "";
			returnResult.WasSuccessful = false;

			PasswordRetrievalResultType typeOfResult = (PasswordRetrievalResultType)result.PasswordRetrievalResultTypeID;
			if (typeOfResult == PasswordRetrievalResultType.Fail || typeOfResult == PasswordRetrievalResultType.NoRegisteredProviders
				|| typeOfResult == PasswordRetrievalResultType.ProviderException || typeOfResult == PasswordRetrievalResultType.Untried)
			{
				returnResult.Message = _translation.GetTerm("ForgotPassword_FailureMessage", "We were unable to retrieve your password.");
			}
			else if (typeOfResult == PasswordRetrievalResultType.InvalidUserIdentifier)
			{
				returnResult.Message = _translation.GetTerm("ForgotPassword_UsernameRequiredError", "We could not find an email associated with the account given.");
			}
			else if (typeOfResult == PasswordRetrievalResultType.Success)
			{
				returnResult.Message = _translation.GetTerm("ForgotPassword_PasswordEmailedMessage", "Your password has been successfully sent to your email.");
				returnResult.WasSuccessful = true;
			}

			return returnResult;
		}

        public IForgotPasswordUIResult ForgotPassword_(string userUniqueIdentifier, string CFP, DateTime birthDay, int siteId)
        {
            IPartialCredentials creds = new PartialCredentials();
            creds.UserUniqueIdentifier = userUniqueIdentifier;
            creds.SiteID = siteId;
            creds.CFP = CFP;
            creds.BirthDay = birthDay;

            IPasswordRetrievalResult result = _authService.RetrieveAccount_(creds);
            IForgotPasswordUIResult returnResult = Create.New<IForgotPasswordUIResult>();
            returnResult.Message = "";
            returnResult.WasSuccessful = false;

            PasswordRetrievalResultType typeOfResult = (PasswordRetrievalResultType)result.PasswordRetrievalResultTypeID;
            if (typeOfResult == PasswordRetrievalResultType.Fail || typeOfResult == PasswordRetrievalResultType.NoRegisteredProviders
                || typeOfResult == PasswordRetrievalResultType.ProviderException || typeOfResult == PasswordRetrievalResultType.Untried)
            {
                returnResult.Message = _translation.GetTerm("ForgotPassword_FailureMessage", "We were unable to retrieve your password.");
            }
            else if (typeOfResult == PasswordRetrievalResultType.InvalidUserIdentifier)
            {
                returnResult.Message = _translation.GetTerm("ForgotPassword_UsernameRequiredError", "We could not find an email associated with the account given.");
            }
            else if (typeOfResult == PasswordRetrievalResultType.Success)
            {
                returnResult.Message = _translation.GetTerm("ForgotPassword_PasswordEmailedMessage", "Your password has been successfully sent to your email.");
                returnResult.WasSuccessful = true;
            }

            return returnResult;
        }

		/// <summary>
		/// Gets the <see cref="ILoginConfiguration"/>
		/// </summary>
		/// <returns>the <see cref="ILoginConfiguration"/></returns>
		public ILoginConfiguration GetConfiguration()
		{
			IAuthenticationConfiguration result = _authService.GetAuthenticationConfiguration();
			ILoginConfiguration returnResult = Create.New<ILoginConfiguration>();
			returnResult.EnableForgotPassword = result.AdminSettings[DefaultAdminSettingKinds.EnableForgotPassword];
			returnResult.ShowUsernameFormFields = result.AdminSettings[DefaultAdminSettingKinds.EnableFormUsernameField];
			switch (result.RegisteredProviders.First())
			{
				case EncoreAuthenticationProviderNames.AccountIDProvider:
					returnResult.PrimaryCredentialType = (int)LoginCredentialTypes.AccountId;
					break;
				case EncoreAuthenticationProviderNames.UsernameProvider:
					returnResult.PrimaryCredentialType = (int)LoginCredentialTypes.Username;
					break;
				case EncoreAuthenticationProviderNames.EmailAddressProvider:
					returnResult.PrimaryCredentialType = (int)LoginCredentialTypes.Email;
					break;
				case EncoreAuthenticationProviderNames.CorporateUsernameProvider:
					returnResult.PrimaryCredentialType = (int)LoginCredentialTypes.CorporateUsername;
					break;
			}

			return returnResult;
		}
	}
}
