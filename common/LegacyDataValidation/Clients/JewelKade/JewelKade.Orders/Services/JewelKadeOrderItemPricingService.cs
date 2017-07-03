using NetSteps.Validation.Handlers.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelKade.Orders.Services
{
    public class JewelKadeOrderItemPricingService : OrderItemPricingService
    {
        public override bool GetHistoricalPrice(int productId, DateTime orderDate, int priceTypeId, int currencyID, out decimal price)
        {
            var priceFound = base.GetHistoricalPrice(productId, orderDate, priceTypeId, currencyID, out price);
            // if wholesale and no price has been found, always return 0 for JewelKade.
            if (!priceFound && priceTypeId == 22)
            {
                price = 0M;
                priceFound = true;
            }
            return priceFound;
        }

        public override bool ShouldMultiplyOrderItemPricesByQuantity
        {
            get
            {
                return true;
            }
        }
    }
}
