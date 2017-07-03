using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Security.Authentication
{
	[ContainerRegister(typeof(IAuthorizationProviders), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public sealed class DefaultAuthorizationProviders : IAuthorizationProviders
	{
		#region Fields

		private ConcurrentBag<IAuthorizationProvider> _providers = new ConcurrentBag<IAuthorizationProvider>();

		#endregion

		public IEnumerable<IAuthorizationProvider> Providers { get { return _providers; } }

		public void RegisterProvider(IAuthorizationProvider provider)
		{
			_providers.Add(provider);
		}
	}
}
