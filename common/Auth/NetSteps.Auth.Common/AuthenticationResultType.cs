using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common
{
	public enum AuthenticationResultType
	{
		Untried,
		Success,
		InvalidUserIdentifier,
		NoRegisteredProviders,
		ProviderException,
		InvalidUserIdentifierFormat,
		InvalidPassword
	}
}
