using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class WarehouseMaterialLacksData
    {
        public string ProductId { get; set; }
        public int PreOrderId { get; set; }
        public int WarehouseMaterialID { get; set; }
        public int Quantity { get; set; }
        public string NameProduct { get; set; }
        public string Motive { get; set; }
    }
}
