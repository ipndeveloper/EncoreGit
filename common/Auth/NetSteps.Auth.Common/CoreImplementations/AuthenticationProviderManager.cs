using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Auth.Common.Configuration;
using System.Reflection;

namespace NetSteps.Auth.Common.CoreImplementations
{
    [ContainerRegister(typeof(IAuthenticationProviderManager), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class AuthenticationProviderManager : IAuthenticationProviderManager
    {
        private AuthenticationConfiguration _configuration;

        public AuthenticationProviderManager(AuthenticationConfiguration configuration)
        {
            for (int i = 0; i < configuration.Providers.Count; i++)
            {
                var authProvider = Container.Current.NewNamed<IAuthenticationProvider>(configuration.Providers[i].Name);
                RegisterProviderKind(authProvider.GetProviderName());
            }
            _configuration = configuration;
        }

        public AuthenticationProviderManager()
        {

        }

        /// <summary>
        /// The registered provider kinds
        /// </summary>
        private readonly List<string> _providerKinds = new List<string>();

        /// <summary>
        /// Registers the kind of the provider.
        /// </summary>
        /// <param name="providerKind">Kind of the authentication provider.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void RegisterProviderKind(string providerKind)
        {
            Contract.Assert(providerKind != null);
            var provider = Container.Current.NewNamed<IAuthenticationProvider>(providerKind);
            Contract.Assert(provider != null);

            if (_providerKinds.Contains(providerKind))
            {
                throw new InvalidOperationException(String.Concat("Provider already registered: ", providerKind));
            }
            _providerKinds.Add(providerKind);
        }

        /// <summary>
        /// Unregisters the adjustment provider.
        /// </summary>
        /// <param name="providerKind">Kind of the authentication provider.</param>
        /// <returns></returns>
        public bool UnregisterAdjustmentProvider(string providerKind)
        {
            Contract.Assert(providerKind != null);
            return _providerKinds.Remove(providerKind);
        }

        /// <summary>
        /// Creates the provider.
        /// </summary>
        /// <param name="providerKind">Kind of the provider.</param>
        /// <returns></returns>
        public IAuthenticationProvider CreateProvider(string providerKind)
        {
            Contract.Assert(providerKind != null);
            if (!_providerKinds.Contains(providerKind))
            {
                throw new Exception(String.Format("The authentication provider kind '{0}' has not been registered with the AuthenticationProviderManager."));
            }
            else
            {
                return Container.Current.NewNamed<IAuthenticationProvider>(providerKind);
            }
        }

        public IEnumerable<string> GetRegisteredProviderNames()
        {
            return _providerKinds.ToArray();
        }

        public IEnumerable<IAuthenticationProvider> GetProviders()
        {
            var providers = new List<IAuthenticationProvider>();
            foreach (var providerKind in _providerKinds)
            {
                providers.Add(CreateProvider(providerKind));
            }
            return providers;
        }

        public IDictionary<string, bool> AdminSettings
        {
            get
            {
                return _configuration.AdminSettings.GetSettings();
            }
        }
    }
}
