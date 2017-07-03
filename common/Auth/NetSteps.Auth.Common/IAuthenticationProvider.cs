using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;
using System.Diagnostics.Contracts;

namespace NetSteps.Auth.Common
{
	[ContractClass(typeof(AuthenticationProviderContract))]
    public interface IAuthenticationProvider
    {
		string GetProviderName();
		IProviderAuthenticationResult Authenticate(ICredentials creds);
		IProviderPasswordRetrievalResult RetrieveAccount(IPartialCredentials creds);
        IProviderPasswordRetrievalResult RetrieveAccount_(IPartialCredentials creds);
	}

	[ContractClassFor(typeof(IAuthenticationProvider))]
	public abstract class AuthenticationProviderContract : IAuthenticationProvider
	{

		public string GetProviderName()
		{
			Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()), "Authentication provider name cannot be null or empty.");
			throw new NotImplementedException();
		}

		public IProviderAuthenticationResult Authenticate(ICredentials creds)
		{
			Contract.Requires(creds != null);
			Contract.Requires(!String.IsNullOrEmpty(creds.UserUniqueIdentifier), "Username cannot be null or empty." );
			Contract.Requires(!String.IsNullOrEmpty(creds.Password), "Password cannot be null or empty.");
			Contract.Ensures(Contract.Result<IProviderAuthenticationResult>() != null);
			throw new NotImplementedException();
		}

		public IProviderPasswordRetrievalResult RetrieveAccount(IPartialCredentials creds)
		{
			Contract.Requires(creds != null);
			Contract.Requires(!String.IsNullOrEmpty(creds.UserUniqueIdentifier), "Username cannot be null or empty.");
			Contract.Ensures(Contract.Result<IProviderPasswordRetrievalResult>() != null);
			throw new NotImplementedException();
		}

        public IProviderPasswordRetrievalResult RetrieveAccount_(IPartialCredentials creds)
        {
            Contract.Requires(creds != null);
            Contract.Requires(!String.IsNullOrEmpty(creds.UserUniqueIdentifier), "Username cannot be null or empty.");
            Contract.Requires(!String.IsNullOrEmpty(creds.CFP), "CFP cannot be null or empty.");
            Contract.Ensures(Contract.Result<IProviderPasswordRetrievalResult>() != null);
            throw new NotImplementedException();
        }
	}
}
