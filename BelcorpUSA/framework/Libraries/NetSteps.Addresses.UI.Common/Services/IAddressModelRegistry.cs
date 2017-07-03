using System;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Encore.Core.Collections;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Common.Services
{
	public interface IAddressModelRegistry : IRegistrar<string, Type>
	{
	}

	public static class IAddresModelRegistryExtensions
	{
		public static bool TryAddressModelInstance(this IAddressModelRegistry registry, string countryCode, out IAddressUIModel model)
		{
			IRegistrationKey<string, Type> registration;
			if (registry.TryGetRegistration(countryCode, out registration))
			{
				var container = Container.Current;
				if (container.IsRoot)
				{
					model = container.NewImplementationOf<IAddressUIModel>(LifespanTracking.External, registration.Handback);
				}
				else
				{
					model = container.NewImplementationOf<IAddressUIModel>(registration.Handback);
				}
				return true;
			}
			model = default(IAddressUIModel);
			return false;
		}

		public static void RegisterAddressModelTypeForCountry<T>(this IAddressModelRegistry registry, string countryCode) where T : IAddressUIModel
		{
			IRegistrationKey<string, Type> registration = null, current;
			Type modelType = typeof(T);

			while (registration == null)
			{
				if (registry.TryGetRegistration(countryCode, out current))
				{
					if (!registry.TryReplaceRegistration(current, countryCode, modelType, out registration))
					{
						throw new RegistrationException();
					}
				}
				else
				{
					registry.TryRegister(countryCode, modelType, out registration);
				}
			}
		}
	}
}
