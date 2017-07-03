using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class GeoScopesByCodesSearchParameters : FilterDateRangePaginatedListParameters<GeoScopesByCodesSearchData>
    {
        public string ValueFrom { get; set; }        
        public string ValueTo { get; set; }
        public bool Except { get; set; }
        public int ShippingOrderTypeID { get; set; }
        public int ShippingOrderTypesGeoScopesByCodeID { get; set; }
    }
}
