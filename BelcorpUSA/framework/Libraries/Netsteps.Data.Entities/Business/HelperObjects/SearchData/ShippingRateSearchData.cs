using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class ShippingRateSearchData
    {
        public int ShippingRateID { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public string ValueName { get; set; }
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public decimal ShippingAmount { get; set; }
        public string RowTotal { get; set; }
    }
}
