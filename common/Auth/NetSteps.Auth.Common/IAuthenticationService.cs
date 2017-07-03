using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Common
{
	public interface IAuthenticationService
	{
		IAuthenticationResult Authenticate(ICredentials creds);
		IAuthenticationResult Authenticate(IAuthenticationToken token);
		IPasswordRetrievalResult RetrieveAccount(IPartialCredentials creds);
        IPasswordRetrievalResult RetrieveAccount_(IPartialCredentials creds);
		IAuthenticationConfiguration GetAuthenticationConfiguration();
	}
}
