using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
	public interface IAuthenticationResult
	{
		int AuthenticationResultTypeID { get; }
		IEnumerable<IProviderAuthenticationResult> ProviderResponseMessages { get; }
		void AddProviderAuthenticationResult(IProviderAuthenticationResult result);
	}
}
