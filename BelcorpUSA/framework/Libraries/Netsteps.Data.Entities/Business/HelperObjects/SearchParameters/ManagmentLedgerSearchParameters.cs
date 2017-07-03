﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ManagmentLedgerSearchParameters : FilterDateRangePaginatedListParameters<ManagmentLedgerSearchData>
    {
        public int? AccountID { get; set; }
        public int? PeriodID { get; set; }
        public int? BonusTypeID { get; set; }
        public decimal? EntryAmount { get; set; }
        public int? EntryReasonID { get; set; }
        public int? EntryOriginID { get; set; }
		public int? EntryTypeID { get; set; }
        public string Order { get; set; }
    }
}
