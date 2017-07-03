using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ShippingCalculatorSearchParameters
    {
        public int OrderTypeID { get; set; }
        public int WareHouseID { get; set; }
        public string PostalCode { get; set; }
        public int OrderValue { get; set; }
        public int ShippingRateGroupID { get; set; } 
        public int LogisticsProviderID { get; set; }
        public int ApprovalDate { get; set; }
        public int ShippingOrderTypeID { get; set; }
    }
}
