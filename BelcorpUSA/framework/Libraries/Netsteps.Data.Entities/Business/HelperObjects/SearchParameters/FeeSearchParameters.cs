using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class FeeSearchParameters : FilterDateRangePaginatedListParameters<FeeSearchData>
    {
        public string DisbursementFeeIDs { get; set; }
        public int DisbursementFeeID { get; set; }
        public int DisbursementFeeTypeID { get; set; }
        public int DisbursementTypeID { get; set; }
        public int CurrencyID { get; set; }
        public double Amount { get; set; }
    }
}
