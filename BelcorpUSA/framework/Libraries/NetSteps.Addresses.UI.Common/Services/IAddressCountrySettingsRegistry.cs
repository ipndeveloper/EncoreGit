using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Encore.Core.Collections;

namespace NetSteps.Addresses.UI.Common.Services
{
	public interface IAddressCountrySettingsRegistry : IRegistrar<string, IAddressCountrySettingsModel>
	{
	}

	public static class AddressContrySettingsRegistryExtensions
	{
		public static void RegisterCountrySettings(this IAddressCountrySettingsRegistry registry, IAddressCountrySettingsModel countrySettings)
		{
			var code = countrySettings.ForCountryCode;

			IRegistrationKey<string, IAddressCountrySettingsModel> registration = null, current;
			while (registration == null)
			{
				if (registry.TryGetRegistration(code, out current))
				{
					if (!registry.TryReplaceRegistration(current, code, countrySettings, out registration))
					{
						throw new RegistrationException();
					}
				}
				else
				{
					registry.TryRegister(code, countrySettings, out registration);
				}
			}
		}

		public static bool TryGetCountrySettings(this IAddressCountrySettingsRegistry registry, string countryCode, out IAddressCountrySettingsModel countrySettings)
		{
			IRegistrationKey<string, IAddressCountrySettingsModel> reg = null;
			bool result = false;

			if (registry.TryGetRegistration(countryCode, out reg))
			{
				countrySettings = reg.Handback;
				result = true;
			}
			else
				countrySettings = null;

			return result;
		}
	}
}
