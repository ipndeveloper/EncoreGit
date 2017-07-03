using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ProductCatalogSearchParameters
    {
        public int ProductCatalogId { get; set; }
        public string ProductCatalogName { get; set; }
        public decimal Retail { get; set; }
        public decimal PreferredCustomer { get; set; }
        public decimal Shippingfee { get; set; }
        public decimal HandlingFee { get; set; }
        public decimal CV { get; set; }
        public decimal HostBase { get; set; }
        public decimal QV { get; set; }
        public decimal WholeSale { get; set; }
    }
}
