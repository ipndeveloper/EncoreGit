using System;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Model;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Store
{
	[ContainerRegister(typeof(IAuthenticationStore), RegistrationBehaviors.Default)]
	public class AuthenticationStore : IAuthenticationStore
	{
		IAccountRepository _accountRepository;
		IUserRepository _userRepository;
        IAccountSuppliedIDRepository _accountSuppliedIDRepository;

        public AuthenticationStore() : this(Create.New<IUserRepository>(), Create.New<IAccountRepository>(), Create.New<IAccountSuppliedIDRepository>()) { }

        public AuthenticationStore(IUserRepository userRepo, IAccountRepository accountRepo, IAccountSuppliedIDRepository accountSuppliedIDRepository)
		{
			_userRepository = userRepo ?? Create.New<IUserRepository>();
			_accountRepository = accountRepo ?? Create.New<IAccountRepository>();
            _accountSuppliedIDRepository = accountSuppliedIDRepository ?? Create.New<IAccountSuppliedIDRepository>();
		}

		public IUserAuthInfo GetUserAuthInfo(string userIdentifier, AuthenticationStoreField field)
		{
			var user = GetUser(userIdentifier, field);

			if (user == null)
			{
				return null;
			}

			var info = Create.New<IUserAuthInfo>();
			info.HashAlgorithm = "SHA512"; // should always be "SHA512";
			info.PasswordSalt = null; // should always be NULL
			info.UserIdentifier = userIdentifier;
			info.UserIdentifierField = field;
			info.PasswordHash = user.PasswordHash;
			info.UserID = user.UserID;
			return info;
		}

		public bool TriggerPasswordRetrieval(string userIdentifier, int siteID, AuthenticationStoreField field)
		{
			var result = false;
			var user = GetUser(userIdentifier, field);
			if (user != null)
			{
				var logic = Create.New<IUserBusinessLogic>();
				result = logic.SendResetPasswordEmail(user.Username, siteID);
			}
			return result;
		}

        public bool TriggerPasswordRetrieval_(string userIdentifier, string CFP, DateTime BirthDay, int siteID, AuthenticationStoreField field)
        {
            var result = false;
            var user = GetUser_(userIdentifier, CFP, BirthDay, field);
            if (user != null)
            {
                var logic = Create.New<IUserBusinessLogic>();
                result = logic.SendResetPasswordEmail(user.Username, siteID);
            }
            return result;
        }

		User GetUser(string userIdentifier, AuthenticationStoreField field)
		{
			User user;
			try
			{
				switch (field)
				{
					case AuthenticationStoreField.AccountID:
						user = _accountRepository.LoadFull(int.Parse(userIdentifier)).User;
						break;
					case AuthenticationStoreField.EmailAddress:
						user = _accountRepository.LoadNonProspectByEmailFull(userIdentifier).User;
						break;
					case AuthenticationStoreField.Username:
					case AuthenticationStoreField.CorporateUsername:
						user = _userRepository.GetByUsername(userIdentifier);
						break;
					default:
						return null;
				}
			}
			catch //if we are unable to find the user for any reason, return null
			{
				return null;
			}
			return user;
		}

        User GetUser_(string userIdentifier, string CFP, DateTime birthDay, AuthenticationStoreField field)
        {
            User user;
            Account account;
            AccountSuppliedID accountSuppliedID;
            try
            {
                switch (field)
                {
                    case AuthenticationStoreField.AccountID:
                        user = _accountRepository.LoadFull(int.Parse(CFP)).User;
                        break;
                    case AuthenticationStoreField.EmailAddress:
                        user = _accountRepository.LoadNonProspectByEmailFull(userIdentifier).User;
                        break;
                    case AuthenticationStoreField.Username:
                    case AuthenticationStoreField.CorporateUsername:
                        user = _userRepository.GetByUsername(userIdentifier);
                        if (user != null)
                        {
                            account = _accountRepository.LoadByUserIdFull_(user.UserID, birthDay);
                            if (account != null)
                            {
                                accountSuppliedID = _accountSuppliedIDRepository.GetByAccountID(account.AccountID, CFP);
                                if (accountSuppliedID == null)
                                {
                                    user = null;
                                }
                            }
                            else
                            {
                                user = null;
                            }
                        }
                        break;
                    default:
                        return null;
                }
            }
            catch //if we are unable to find the user for any reason, return null
            {
                return null;
            }
            return user;
        }

		/// <summary>
		/// Updates the login result on the user.
		/// Successful and InvalidPassword are the only two resultTypes that will be acted upon.
		/// </summary>
		/// <param name="result"></param>
		public void RecordLoginResultInfo(IProviderAuthenticationResult result)
		{
			bool saveUser = true;

			if (result.UserID <= 0)
			{
				return;
			}

			User user = User.Load(result.UserID);
			if (user == null)
			{
				return;
			}

			switch (result.AuthenticationResultTypeID)
			{
				case (int)AuthenticationResultType.Success:
					++user.TotalLoginCount;
					user.LastLogin = DateTime.Now; //This is converted to local time inside of the setter
					user.ConsecutiveFailedLogins = 0;
					break;
				case (int)AuthenticationResultType.InvalidPassword:
					++user.ConsecutiveFailedLogins;
					user.LoginMessage = result.Message;
					break;
				default:
					saveUser = false;
					break;
			}

			if (saveUser)
			{
				user.Save();
			}
		}
	}
}
