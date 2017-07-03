using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class ProductCartDTO
    {
        public int ProductID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string RetailPerItem { get; set; }
        public decimal PricePerItem { get; set; }
        public string AdjustedItemPrice { get; set; }
        public string PreAdjustedItemPrice { get; set; }
        public string AdjustedCommissionTotal { get; set; }
        public string PreadjustedCommissionTotal { get; set; } 
        public int Quantity { get; set; }
        public string TotalQV { get; set; }
        public string TotalPrice { get; set; }

    }
}
