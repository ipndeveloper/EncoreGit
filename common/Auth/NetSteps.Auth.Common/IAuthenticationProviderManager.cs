using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common
{
	public interface IAuthenticationProviderManager
	{

		/// <summary>
		/// Unregisters the adjustment provider.
		/// </summary>
		/// <param name="providerKind">Kind of the provider.</param>
		/// <returns></returns>
		bool UnregisterAdjustmentProvider(string providerKind);

		/// <summary>
		/// Creates the provider.
		/// </summary>
		/// <param name="providerKind">Kind of the provider.</param>
		/// <returns></returns>
		IAuthenticationProvider CreateProvider(string providerKind);

		/// <summary>
		/// Gets the registered provider names.
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetRegisteredProviderNames();

		/// <summary>
		/// Gets the authentication providers currently registered with the configuration.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IAuthenticationProvider> GetProviders();

        /// <summary>
        /// The administrative configuration settings defined for authentication.
        /// </summary>
        IDictionary<string, bool> AdminSettings { get; }
	}
}
