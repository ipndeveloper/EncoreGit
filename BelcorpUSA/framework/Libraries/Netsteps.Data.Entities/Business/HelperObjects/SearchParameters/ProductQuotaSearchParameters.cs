using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ProductQuotaSearchParameters : FilterDateRangePaginatedListParameters<ProductQuotaSearchData>
    {
        public string Order { get; set; }
    }
}
