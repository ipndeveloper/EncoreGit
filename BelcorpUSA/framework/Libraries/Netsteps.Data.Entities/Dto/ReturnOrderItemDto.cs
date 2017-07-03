using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class ReturnOrderItemDto
    {
        public int? ParentOrderItemID { get; set; }
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public string SKU { get; set; }
        public int ParentQuantity { get; set; }
        public int Quantity { get; set; }
        public int QuantityOrigen { get; set; }
        public string ItemPrice { get; set; }
        public bool HasComponents { get; set; }
        public bool AllHeader { get; set; }
        public bool IsChild { get; set; }
    }
}
