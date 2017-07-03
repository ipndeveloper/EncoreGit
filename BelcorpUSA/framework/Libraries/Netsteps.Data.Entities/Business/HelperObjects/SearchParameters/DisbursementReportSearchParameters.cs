﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class DisbursementReportSearchParameters : FilterDateRangePaginatedListParameters<DisbursementReportSearchData>
    {
        public int? AccountID { get; set; }
        public int? PeriodID { get; set; }
        public int? DisbursementStatusID { get; set; }
        public string Order { get; set; }
        
    }
}
