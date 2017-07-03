using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class OrderReturnTable
    {
        public int OrderItemID { get; set; }
        public string SKU { get; set; }
        public int ProductID { get; set; }
        public int QuantityReturn { get; set; }
        public bool BlockHead { get; set; }
    }

    public class OrderReturnDetailsTable
    {
        public int OrderItemID { get; set; }
        public int OrderStatusID { get; set; }
        public string SKU { get; set; }
        public int ProductID { get; set; }
        public int QuantityReturn { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderReturnChildrenTable
    {
        public int OrderItemID { get; set; }
        public string MaterialSKU { get; set; }
        public int MaterialID { get; set; }
        public int QuantityReturn { get; set; }

    }
}
