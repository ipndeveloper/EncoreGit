using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ClaimedOrderItem
    {

        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int OrderCustomerID { get; set; }

        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public int OrderQuantity { get; set; }
        public int ClaimQuantity { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalChaim { get; set; }

        public string CultureInfo { get; set; }
        public string CurrencyCode { get; set; }

    }
}
