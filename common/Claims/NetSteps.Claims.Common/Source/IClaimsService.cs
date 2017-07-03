using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IClaimsService
	{
		IRealm RegisterRealm(string realmName);

		IApplication RegisterApplication(string applicationName, string claimSetResolutionUrl);

		IRealmApplication RegisterApplicationWithRealm(IRealm realm, IApplication application);

		IRegisteredClaimSet RegisterClaimSet(IApplication application, string claimSetName, IEnumerable<string> claims);

		IRegisteredIdentity RegisterIdentity(string userName, string tenant, string password);

		IRealmApplicationUser RegisterUserWithRealmApplication(IRegisteredIdentity user, IRealmApplication realmApplication);
	}
}
