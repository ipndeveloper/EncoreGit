using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class CommissionDetailSearchData
    {

        public int SponsorID { get; set; }
        public string SponsorName { get; set; }
        public int AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CommissionType { get; set; }          // BonusType.BonusCode
        public string CommissionName { get; set; }          // BonusType.Name
        public int OrderNumber { get; set; }
        public decimal CommissionableValue { get; set; }    // BonusDetails.QV
        public decimal Percentage { get; set; }
        public decimal PayoutAmount { get; set; }
        public int PeriodID { get; set; }

        public int TotalPages { get; set; }
        public int TotalRows { get; set; }

    }
}
