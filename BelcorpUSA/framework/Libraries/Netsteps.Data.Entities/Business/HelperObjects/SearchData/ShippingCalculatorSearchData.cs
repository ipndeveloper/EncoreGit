using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class ShippingCalculatorSearchData
    {
        public class GetShipping
        {
            public int ID { get; set; }
            public string Nombre { get; set; }
            public int SortIndex { get; set; }
            public int LogisticsProviderID { get; set; }
            public int ShippingOrderTypeID { get; set; }
            public int OrderTypeID  { get; set; }
            public string Name { get; set; }
            public int DaysForDelivery { get; set; }
            public int ShippingMethodID { get; set; }
            public string DateEstimated { get; set; }
        }

        public class GetEstimatedDeliveryDate
        {
            public int PostalCode { get; set; }
            public int RouteID { get; set; }
            public string monday { get; set; }
            public string tuesday { get; set; }
            public string Wednesday { get; set; }
            public string Thursday { get; set; }
            public string friday { get; set; }
            public string saturday { get; set; }
            public string sunday { get; set; }
            public string WorkInSaturdays { get; set; }
            public int WorkInSundays { get; set; }
            public int WorkInHolidays { get; set; }
            public int DaysForDelivery { get; set; }
        }

        public class GetShippingRateGroup
        {
            public int ShippingRateID { get; set; }
            public int ShippingRateGroupID { get; set; }
        }

        public class GetOrderDetails
        {
            public int PreOrderId { get; set; }
            public int WareHouseID { get; set; }
        }

        public class GetProductQuantity
        {
            public int ProductID { get; set; }
            public int Quantity { get; set; }
        }
    }
}
