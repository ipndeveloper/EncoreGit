using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;

namespace NetSteps.Validation.Handlers.Services.ServiceModels
{
    public class ProductPriceType : IProductPriceType
    {
        public string Name { get; set; }

        public int PriceTypeID { get; set; }

        public PriceTypeCategory Category { get; set; }
    }
}
