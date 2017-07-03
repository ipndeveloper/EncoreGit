using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common
{
	public enum PasswordRetrievalResultType
	{
		Untried,
		Success,
		Fail,
		NoRegisteredProviders,
		ProviderException,
		InvalidUserIdentifier
	}
}
