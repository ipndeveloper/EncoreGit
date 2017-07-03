using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
	public interface IProviderAuthenticationResult
	{
		string ProviderName { get; }
		int AuthenticationResultTypeID { get; }
		Exception AuthenticationException { get; }
		string Message { get; }
		int UserID { get; }
	}
}
