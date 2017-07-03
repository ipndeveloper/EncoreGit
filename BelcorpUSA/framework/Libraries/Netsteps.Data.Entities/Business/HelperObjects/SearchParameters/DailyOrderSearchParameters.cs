using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class DailyOrderSearchParameters : FilterDateRangePaginatedListParameters<DailyOrderSearchData>
	{
        public bool GetAll { get; set; }
        public int LanguageID { get; set; }

        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? CompleteDateStart { get; set; }
        public DateTime? CompleteDateEnd { get; set; }
        public decimal? SubTotalMin { get; set; }
        public decimal? SubTotalMax { get; set; }
        public decimal? GrandTotalMin { get; set; }
        public decimal? GrandTotalMax { get; set; }
	}
}
