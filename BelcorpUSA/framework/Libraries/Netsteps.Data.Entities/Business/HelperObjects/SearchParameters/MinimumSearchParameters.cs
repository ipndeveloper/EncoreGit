using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class MinimumSearchParameters : FilterDateRangePaginatedListParameters<MinimumSearchData>
    {
        public string DisbursementMinimumIDs { get; set; }
        public int DisbursementMinimumID { get; set; }
	    public double MinimumAmount { get; set; }
	    public int CurrencyID { get; set; }
        public int DisbursementTypeID { get; set; }
    }
}
