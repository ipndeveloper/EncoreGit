using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Security.Authentication
{
	public interface IAuthorizationProviders
	{
		IEnumerable<IAuthorizationProvider> Providers { get; }

		void RegisterProvider(IAuthorizationProvider provider);
	}
}
