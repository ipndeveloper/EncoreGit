using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Auth.Common.CoreImplementations
{
	[ContainerRegister(typeof(IAuthenticationStore), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class DefaultAuthenticationStore : IAuthenticationStore
	{
		public Model.IUserAuthInfo GetUserAuthInfo(string userIdentifier, AuthenticationStoreField field)
		{
			throw new NotSupportedException("Please ensure that a proper authentication store is wired up.  This implementation is for test purposes only.");
		}

		public bool TriggerPasswordRetrieval(string userIdentifier, int siteID, AuthenticationStoreField field)
		{
			throw new NotSupportedException("Please ensure that a proper authentication store is wired up.  This implementation is for test purposes only.");
		}
	}
}
