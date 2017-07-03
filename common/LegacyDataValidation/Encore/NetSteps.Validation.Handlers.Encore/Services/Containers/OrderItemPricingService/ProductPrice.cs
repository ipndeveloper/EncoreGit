using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Services.Containers.OrderItemPricingService
{
    public class ProductPrice
    {
        public ProductPrice(int productPriceTypeID, decimal price, int currencyID, DateTime effectiveDate)
        {
            ProductPriceTypeID = productPriceTypeID;
            Price = price;
            CurrencyID = currencyID;
            EffectiveDate = effectiveDate;
        }

        public int ProductPriceTypeID { get; private set; }
        public decimal Price { get; private set; }
        public int CurrencyID { get; private set; }
        public DateTime EffectiveDate { get; private set; }

    }
}
