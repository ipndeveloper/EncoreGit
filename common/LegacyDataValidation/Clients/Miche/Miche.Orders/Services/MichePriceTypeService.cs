using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Services;

namespace Miche.Orders.Services
{
    public class MichePriceTypeService : PriceTypeService
    {
        public MichePriceTypeService(Func<IProductPriceType> priceTypeFactory)
            : base(priceTypeFactory)
        {
            base.AddPriceType(NetSteps.Validation.Handlers.Common.Services.ServiceModels.PriceTypeCategory.Volume, "IPV", 37);
        }
    }
}
