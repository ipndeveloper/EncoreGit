using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Services.Containers.OrderItemPricingService
{
    public class OrderItemPriceManager
    {
        public OrderItemPriceManager()
        {
            _productPrices = new Dictionary<int, List<ProductPrice>>();
        }

        private readonly IDictionary<int, List<ProductPrice>> _productPrices;

        public void AddPrice(int productID, int productPriceTypeID, decimal price, int currencyID, DateTime effectiveDate)
        {
            if (!_productPrices.ContainsKey(productID))
            {
                _productPrices.Add(productID, new List<ProductPrice>());
            }
            _productPrices[productID].Add(new ProductPrice(productPriceTypeID, price, currencyID, effectiveDate));
        }

        internal List<ProductPrice> GetPrices(int productId)
        {
            return _productPrices[productId];
        }
    }
}
