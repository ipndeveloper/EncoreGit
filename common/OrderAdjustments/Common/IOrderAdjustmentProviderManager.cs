using System;
using System.Collections.Generic;

namespace NetSteps.OrderAdjustments.Common
{
    public interface IOrderAdjustmentProviderManager
    {
        /// <summary>
        /// Registers an IOrderAdjustmentProvider with the adjustment provider manager.
        /// </summary>
        /// <param name="provider">The Provider.</param>
        void RegisterAdjustmentProvider(IOrderAdjustmentProvider provider);

        /// <summary>
        /// Unregisters an IOrderAdjustmentProvider with the adjustment provider manager.
        /// </summary>
        /// <param name="provider">The Provider being removed.</param>
        /// <returns></returns>
        bool UnregisterAdjustmentProvider(IOrderAdjustmentProvider provider);

        /// <summary>
        /// Gets a provider by its ProviderKey.
        /// </summary>
        /// <param name="providerKey">The provider's key.</param>
        /// <returns></returns>
        IOrderAdjustmentProvider GetProvider(string providerKey);

        /// <summary>
        /// Gets all providers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOrderAdjustmentProvider> GetAllProviders();
    }
}
