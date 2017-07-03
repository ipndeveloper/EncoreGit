using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustmentProviderManager : IOrderAdjustmentProviderManager
    {
        protected internal Dictionary<string, IOrderAdjustmentProvider> _providers { get; set; }
        
        public MockOrderAdjustmentProviderManager(int productID, int accountID)
        {
            _providers = new Dictionary<string, IOrderAdjustmentProvider>();
            
            adjustments = new List<IOrderAdjustmentProfile>();
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItemWithQuantity2, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedProduct1PriceBy14Percent, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedProduct1PriceBy16Flat, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedShippingTotalBy10Flat, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedShippingTotalBy20Percent, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy23Percent, productID, accountID));
            adjustments.Add(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy24Flat, productID, accountID));
        }

        public void RegisterAdjustmentProvider(IOrderAdjustmentProvider Provider)
        {
            _providers.Add(Provider.GetProviderKey(), Provider);
        }

        public bool UnregisterAdjustmentProvider(IOrderAdjustmentProvider Provider)
        {
            string safeKey = Provider.GetProviderKey();
            if (_providers.Keys.Contains(safeKey))
            {
                _providers.Remove(safeKey);
                return true;
            }
            return false;
        }

        private List<IOrderAdjustmentProfile> adjustments { get; set; }
        public IEnumerable<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(IOrderContext order)
        {
            return adjustments;
        }

        public IOrderAdjustmentProvider GetProvider(string ProviderKey)
        {
            if (_providers.ContainsKey(ProviderKey))
                return _providers[ProviderKey];
            return null;
        }


        public IEnumerable<IOrderAdjustmentProvider> GetAllProviders()
        {
            return _providers.Values.ToList();
        }
    }
}
