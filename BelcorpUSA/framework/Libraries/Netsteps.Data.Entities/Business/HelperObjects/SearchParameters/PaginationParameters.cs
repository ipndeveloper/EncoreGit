using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class PaginationParameters : FilterDateRangePaginatedListParameters<listdispatchGet>
    {

        public string Description { get; set; }
        public int UserID { get; set; }
        public string Order { get; set; }
        public string PeriodStart { get; set; }
        public string PeriodEnd { get; set; }
        public string SKU { get; set; }
    }
}
