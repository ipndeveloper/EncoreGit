﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class WareHouseMaterialSearchParameters : FilterDateRangePaginatedListParameters<WareHouseMaterialSearchData>
    {
        public int? WareHouseID { get; set; }
        public int? ProductID { get; set; }
        public int? MaterialID { get; set; }
        public int WareHouseMaterialID { get; set; }
        public int Eval { get; set; }
        public int QuantityField { get; set; } 
        public int QuantityBefore { get; set; } 
        public int InventiryMovementTypeID { get; set; }
        public int QuantityOnHandAfter { get; set; }
        public int QuantityMov { get; set; }
        public decimal AverageCost { get; set; } 
        public string Description { get; set; }
        public int UserID { get; set; }
        public string Order { get; set; }
    }
}
