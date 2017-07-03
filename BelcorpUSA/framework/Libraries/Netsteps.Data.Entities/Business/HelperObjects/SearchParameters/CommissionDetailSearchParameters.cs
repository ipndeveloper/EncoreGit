using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class CommissionDetailSearchParameters
    {

        public int PeriodID { get; set; }
        public string CommissionType { get; set; }
        public int AccountID { get; set; }

        public Constants.SortDirection SortDirection { get; set; }
        public string OrderColumn { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
