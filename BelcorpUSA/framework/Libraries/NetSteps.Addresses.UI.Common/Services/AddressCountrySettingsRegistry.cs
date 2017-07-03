using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Encore.Core.Collections;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Common.Services
{
	public static class AddressCountrySettingsRegistry
	{
		public static IAddressCountrySettingsRegistry Instance
		{
			get
			{
				return Create.New<IAddressCountrySettingsRegistry>();
			}
		}
	}

	[ContainerRegister(typeof(IAddressCountrySettingsRegistry), RegistrationBehaviors.IfNoneOther)]
	public class DefaultAddressCountrySettingsRegistry : Registrar<string, IAddressCountrySettingsModel>, IAddressCountrySettingsRegistry
	{
	}
}
