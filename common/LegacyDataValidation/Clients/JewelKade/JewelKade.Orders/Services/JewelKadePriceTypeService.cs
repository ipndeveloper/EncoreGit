using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Services;

namespace JewelKade.Orders.Services
{
    public class JewelKadePriceTypeService : PriceTypeService
    {
        public JewelKadePriceTypeService(Func<IProductPriceType> priceTypeFactory)
            : base(priceTypeFactory)
        {
            AddPriceType(PriceTypeCategory.Currency, "RPVDiscount", 27);
            AddPriceType(PriceTypeCategory.Currency, "Club JK Member", 2);
        }
    }
}
