using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class CommissionSearchData
    {

        public int PeriodID { get; set; }
        public int AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CommissionType { get; set; }      // BonusType.BonusCode
        public string CommissionName { get; set; }      // BonusType.Name
        public decimal CommissionAmount { get; set; }   // BonusValues.BonusAmount

        public int TotalPages { get; set; }
        public int TotalRows { get; set; }

    }
}
