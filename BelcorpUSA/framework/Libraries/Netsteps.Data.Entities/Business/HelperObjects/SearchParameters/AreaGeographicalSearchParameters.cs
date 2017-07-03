using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class AreaGeographicalSearchParameters : FilterDateRangePaginatedListParameters<ZonesData>
    {
        
        public int GeoScopeID { get; set; }
        public int ScopeLevelID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Except { get; set; }
        public int ShippingOrderTypeID { get; set; }
    }
}
