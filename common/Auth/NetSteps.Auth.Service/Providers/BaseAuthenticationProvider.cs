using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Service.Providers
{
	public abstract class BaseAuthenticationProvider : IAuthenticationProvider
	{
		private readonly Lazy<IAuthenticationStore> __storeFactory;
		protected IAuthenticationStore _store
		{
			get
			{
				return __storeFactory.Value;
			}
		}

		protected BaseAuthenticationProvider(Func<IAuthenticationStore> storeConstructor)
		{
			__storeFactory = new Lazy<IAuthenticationStore>(storeConstructor);
		}

		public abstract string GetProviderName();

		public abstract IProviderAuthenticationResult Authenticate(Common.Model.ICredentials creds);

		public abstract IProviderPasswordRetrievalResult RetrieveAccount(Common.Model.IPartialCredentials creds);

        public abstract IProviderPasswordRetrievalResult RetrieveAccount_(Common.Model.IPartialCredentials creds);
	}
}
