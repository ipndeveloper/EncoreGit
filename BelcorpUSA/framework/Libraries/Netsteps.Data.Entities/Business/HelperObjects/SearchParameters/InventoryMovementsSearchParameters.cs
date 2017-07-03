using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;

namespace NetSteps.Data.Entities.Business
{
    public class InventoryMovementsSearchParameters : FilterDateRangePaginatedListParameters<WarehouseInventoryMovementsSearchData>
    {
        static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int MaterialID { get; set; }
        public int InventoryMovementTypeID { get; set; }
    }
}
