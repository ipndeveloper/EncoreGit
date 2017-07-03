using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class WarehouseMaterialAllocationLogsParameters
    {
        public int WareHouseMaterialId { get; set; }
        public int InventoryMovementTypeID { get; set; }
        public int QuantityAllocatedBefore { get; set; }
        public int QuantityMov { get; set; }
        public int QuantityAllocatedAfter { get; set; }
        public double AverageCost { get; set; }
        public string Description { get; set; }
        public int userID { get; set; } 
        public int PreOrderID { get; set; } 
    }
}
