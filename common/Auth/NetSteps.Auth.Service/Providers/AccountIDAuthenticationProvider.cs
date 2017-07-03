using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Model;
using System.Text.RegularExpressions;
using NetSteps.Auth.Service.Model.Concrete;
using NetSteps.Security;

namespace NetSteps.Auth.Service.Providers
{
	public class AccountIDAuthenticationProvider : BaseAuthenticationProvider, IAuthenticationProvider
	{
		private AuthenticationStoreField _storeField = AuthenticationStoreField.AccountID;

		public AccountIDAuthenticationProvider(Func<IAuthenticationStore> storeConstructor) : base(storeConstructor)
		{
		}

		public override string GetProviderName()
		{
			return EncoreAuthenticationProviderNames.AccountIDProvider;
		}

		public override IProviderAuthenticationResult Authenticate(ICredentials creds)
		{
			var result = new ProviderAuthenticationResult();
			result.ProviderName = GetProviderName();
			
			// validation check
			int accountID = 0;
			if (!int.TryParse(creds.UserUniqueIdentifier, out accountID))
			{
				result.AuthenticationResultTypeID = (int)AuthenticationResultType.InvalidUserIdentifierFormat;
				result.Message = String.Format("Username must be a number.  Received '{0}'", creds.UserUniqueIdentifier);
			}
			else
			{
				var userInfo = _store.GetUserAuthInfo(creds.UserUniqueIdentifier, _storeField);
				if (userInfo != null)
				{
					var algorithm = SimpleHash.Algorithm.SHA512;
					Enum.TryParse<SimpleHash.Algorithm>(userInfo.HashAlgorithm, out algorithm);
					// compare hash with userInfo hashed password
					if (SimpleHash.VerifyHash(creds.Password, algorithm, userInfo.PasswordHash))
					{
						result.AuthenticationResultTypeID = (int)AuthenticationResultType.Success;
					}
					else
					{
						result.AuthenticationResultTypeID = (int)AuthenticationResultType.InvalidPassword;
					}
				}
				else
				{
					result.AuthenticationResultTypeID = (int)AuthenticationResultType.InvalidUserIdentifier;
				}
				result.UserID = userInfo.UserID;
				_store.RecordLoginResultInfo(result);
			}
			return result;
		}

		public override IProviderPasswordRetrievalResult RetrieveAccount(Common.Model.IPartialCredentials creds)
		{
			var result = new ProviderPasswordRetrievalResult();

			if (_store.TriggerPasswordRetrieval(creds.UserUniqueIdentifier, creds.SiteID, _storeField))
			{
				result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.Success;
			}
			else
			{
				result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.Fail;
			}
			return result;
		}

        public override IProviderPasswordRetrievalResult RetrieveAccount_(Common.Model.IPartialCredentials creds)
        {
            var result = new ProviderPasswordRetrievalResult();

            if (_store.TriggerPasswordRetrieval_(creds.UserUniqueIdentifier, creds.CFP, creds.BirthDay, creds.SiteID, _storeField))
            {
                result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.Success;
            }
            else
            {
                result.PasswordRetrievalResultTypeID = (int)PasswordRetrievalResultType.Fail;
            }
            return result;
        }
	}
}
