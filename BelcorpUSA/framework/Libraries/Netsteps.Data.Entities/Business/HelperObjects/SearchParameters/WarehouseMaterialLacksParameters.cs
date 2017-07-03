using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class WarehouseMaterialLacksParameters
    {
        public int WarehouseMaterialID { get; set; }
        public int PreOrderId { get; set; }
        public int OrderId { get; set; }
        public int QuantityLack { get; set; }
        public int ProductId { get; set; }
        public int OfferType { get; set; }
        public int ProductPriceTypeId { get; set; }
        public string Motive { get; set; }
    }
}
