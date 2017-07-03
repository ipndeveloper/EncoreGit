using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.BatchProcess.Common;

namespace NetSteps.Validation.Handlers.Encore.Common.Services
{
    public interface IOrderItemPricingService : IDependentDataService
    {
        bool GetHistoricalPrice(int productId, DateTime orderDate, int priceTypeId, int currencyID, out decimal price);
        bool ShouldMultiplyOrderItemPricesByQuantity { get; }
    }
}
