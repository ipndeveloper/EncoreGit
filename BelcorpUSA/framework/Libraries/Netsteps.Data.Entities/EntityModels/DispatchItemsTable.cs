using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class DispatchItemsTable
    {
        public int DispatchID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int Quantity2 { get; set; }
        public string SKU { get; set; }
    }

    public class DispatchItemsQuery
    {
        public int DispatchID { get; set; }
        public int ProductID { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public int Quantity2 { get; set; }
        public string SKU { get; set; }
    }
}
