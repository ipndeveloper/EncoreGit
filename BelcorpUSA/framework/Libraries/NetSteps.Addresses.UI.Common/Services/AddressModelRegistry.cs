using System;
using NetSteps.Encore.Core.Collections;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Common.Services
{
	public static class AddressModelRegistry
	{
		public static IAddressModelRegistry Instance
		{
			get
			{
				return Create.New<IAddressModelRegistry>();
			}
		}
	}

	[ContainerRegister(typeof(IAddressModelRegistry), RegistrationBehaviors.IfNoneOther)]
	public class DefaultAddressModelRegistry : Registrar<string, Type>, IAddressModelRegistry
	{
	}
}
