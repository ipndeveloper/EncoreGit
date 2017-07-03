using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class PricesPerCatalogsData
    {
        //Developed by Kelvin Lopez C. - CSTI
        public int CatalogID { get; set; }
        public string Name { get; set; }
    }
    public class ProductPricesCatalog
    {
        public int productId { get; set; }
        public int currencyId { get; set; }
        public int catalogID { get; set; }
        public int ProductPriceTypeID { get; set; }
        public decimal Price { get; set; }
        //Dictionary<int, decimal> pricesd { get; set; }
    }
}
