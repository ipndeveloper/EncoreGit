using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.Data.Entities.Factories
{
	[ContainerRegister(typeof(ITokenValueProviderFactory), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class CompositeTokenValueProviderFactory : ITokenValueProviderFactory
	{
		private ITokenValueProvider _theProvider;

		public CompositeTokenValueProviderFactory()
		{
			var internalProviders = new ITokenValueProvider[]{
				new ContextTokenValueProvider()
			};

			_theProvider = new CompositeTokenValueProvider(internalProviders);
		}

		public ITokenValueProvider GetTokenProvider(NetSteps.Common.Constants.TokenValueProviderType tokenValueProviderType)
		{
			return _theProvider;
		}
	}
}
