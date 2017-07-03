using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common.Component
{
    public class OrderAdjustmentProviderManager : IOrderAdjustmentProviderManager
    {
        ConcurrentDictionary<string, IOrderAdjustmentProvider> _providers = new ConcurrentDictionary<string, IOrderAdjustmentProvider>();

        protected IOrderAdjustmentHandler Handler { get; private set; }

        public OrderAdjustmentProviderManager()
        {
        }

        public void RegisterAdjustmentProvider(IOrderAdjustmentProvider provider)
        {
            Contract.Assert(provider != null);

            var key = provider.GetProviderKey();
            if (!_providers.TryAdd(key, provider))
            {
                throw new InvalidOperationException(String.Concat("Provider already registered: ", key));
            }
        }

        public bool UnregisterAdjustmentProvider(IOrderAdjustmentProvider provider)
        {
            Contract.Assert(provider != null);

            IOrderAdjustmentProvider unused;
            return _providers.TryRemove(provider.GetProviderKey(), out unused);
        }

        public IOrderAdjustmentProvider GetProvider(string providerKey)
        {
            Contract.Assert(providerKey != null);

            IOrderAdjustmentProvider result;
            _providers.TryGetValue(providerKey, out result);
            return result;
        }

        public IEnumerable<IOrderAdjustmentProvider> GetAllProviders()
        {
            return _providers.Values.ToReadOnly();
        }
    }
}
