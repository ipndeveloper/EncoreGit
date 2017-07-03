using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Model;
using NetSteps.Security;
using NetSteps.Auth.Service.Model.Concrete;

namespace NetSteps.Auth.Service.Providers
{
	public class CorporateUsernameAuthenticationProvider : BaseAuthenticationProvider, IAuthenticationProvider
	{
		private AuthenticationStoreField _storeField = AuthenticationStoreField.CorporateUsername;

		public CorporateUsernameAuthenticationProvider(Func<IAuthenticationStore> storeConstructor) : base(storeConstructor)
		{
		}

		public override string GetProviderName()
		{
			return EncoreAuthenticationProviderNames.CorporateUsernameProvider;
		}

		public override IProviderAuthenticationResult Authenticate(ICredentials creds)
		{
			var result = new ProviderAuthenticationResult();
			result.ProviderName = GetProviderName();

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
