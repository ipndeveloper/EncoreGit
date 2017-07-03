using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.Extensibility.Core
{
    /// <summary>
    /// Provides base functionality for registering providers of extensions to extensible data objects.  Wired up to the IOC automatically as part of the module wireup.
    /// </summary>
    [ContainerRegister(typeof(IDataObjectExtensionProviderRegistry), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.FinalSingletoon)]
    public class DataObjectExtensionProviderRegistry : IDataObjectExtensionProviderRegistry
    {
        /// <summary>
        /// The dictionary of providers registered with a string representation of their types as a key.
        /// </summary>
        private readonly ConcurrentDictionary<string, RegistrationRecord> _providerConstructors = new ConcurrentDictionary<string, RegistrationRecord>();

        /// <summary>
        /// A provider registration record.
        /// </summary>
        abstract class RegistrationRecord
        {
            /// <summary>
            /// Contains a list of keys for the type.
            /// </summary>
            public List<string> keys = new List<string>();
            /// <summary>
            /// An untyped provider.
            /// </summary>
            /// <returns></returns>
            public abstract IDataObjectExtensionProvider UntypedConstruct();
        }

        /// <summary>
        /// Provides an abstraction for getting an untyped provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class RegistrationRecord<T> : RegistrationRecord
            where T : IDataObjectExtensionProvider
        {
            public override IDataObjectExtensionProvider UntypedConstruct()
            {
                return Create.New<T>();
            }
        }

        /// <summary>
        /// Retrieves the extension provider.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IDataObjectExtensionProvider RetrieveExtensionProvider(string key)
        {
            Contract.Assert(!String.IsNullOrEmpty(key));

            RegistrationRecord constructor;

            if (_providerConstructors.TryGetValue(key, out constructor))
            {
                return constructor.UntypedConstruct();
            }
            return null;
        }


        public TProvider RetrieveExtensionProvider<TProvider>(string key)
            where TProvider : IDataObjectExtensionProvider
        {
            Contract.Assert(!String.IsNullOrEmpty(key));

            RegistrationRecord providerRecord;

            if (_providerConstructors.TryGetValue(key, out providerRecord))
            {
                var provider = providerRecord.UntypedConstruct();
                Contract.Assert(provider != null);
                Contract.Assert(typeof(TProvider).IsAssignableFrom(provider.GetType()));
                return (TProvider)(object)providerRecord.UntypedConstruct();
            }
            return default(TProvider);
        }

        /// <summary>
        /// Registers the extension provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="providerKey">The provider key.</param>
        public void RegisterExtensionProvider<T>(string providerKey)
                where T : IDataObjectExtensionProvider
        {
            if (!Container.Current.Registry.IsTypeRegistered<T>())
            {
                Container.Current
                     .ForType<T>()
                     .Register<T>()
                     .ResolveAsSingleton()
                     .End();
            }
            if (_providerConstructors.TryAdd(providerKey, new RegistrationRecord<T>()))
            {
                _providerConstructors[providerKey].keys.Add(providerKey);
            }
        }

        /// <summary>
        /// Registers the provided type for registered provider.
        /// </summary>
        /// <param name="ExtensionTypeName">Name of the extension type.</param>
        /// <param name="providerKey">The provider key.</param>
        public void RegisterProvidedTypeForRegisteredProvider(string ExtensionTypeName, string providerKey)
        {
            if (!_providerConstructors.ContainsKey(providerKey))
                throw new NotSupportedException("Cannot register a provided type if the specified providerKey is not registered.");

            if (_providerConstructors.TryAdd(ExtensionTypeName, _providerConstructors[providerKey]))
                _providerConstructors[ExtensionTypeName].keys.Add(ExtensionTypeName);
        }

        /// <summary>
        /// Retrieves the extension provider for string representation of the registered provided type.
        /// </summary>
        /// <param name="extensionTypeName">Name of the extension type.</param>
        /// <returns></returns>
        public IDataObjectExtensionProvider RetrieveExtensionProviderForRegisteredProvidedType(string extensionTypeName)
        {
            RegistrationRecord provider;
            bool found = _providerConstructors.TryGetValue(extensionTypeName, out provider);
            if (found)
                return provider.UntypedConstruct();
            else
                return null;
        }

        /// <summary>
        /// Retrieves the extension provider for string representation of the registered provided type.
        /// </summary>
        /// <param name="extensionTypeName">Name of the extension type.</param>
        /// <returns></returns>
        public TProvider RetrieveExtensionProviderForRegisteredProvidedType<TProvider>(string extensionTypeName)
            where TProvider : IDataObjectExtensionProvider
        {
            RegistrationRecord providerRecord;
            bool found = _providerConstructors.TryGetValue(extensionTypeName, out providerRecord);
            if (found)
            {
                var provider = providerRecord.UntypedConstruct();
                Contract.Assert(provider != null);
                Contract.Assert(typeof(TProvider).IsAssignableFrom(provider.GetType()));
                return (TProvider)(object)provider;
            }
            else
                return default(TProvider);
        }
    }
}
