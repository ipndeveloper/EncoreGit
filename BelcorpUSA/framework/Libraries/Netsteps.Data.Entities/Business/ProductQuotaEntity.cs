using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class ProductQuotaEntity
    {

        public int RestrictionID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RestrictionType { get; set; }
        public bool Active { get; set; }

        public int ProductID { get; set; }
        public string ProductSKU { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int StartPeriodID { get; set; }
        public int EndPeriodID { get; set; }

        public List<int> PaidAsTitlesIDs { get; set; }
        public List<int> RecognizedTitlesIDs { get; set; }
        public List<int> AccountTypeIDs { get; set; }
        public List<string> AccountIDs { get; set; }

        public ProductQuotaEntity()
        {
            PaidAsTitlesIDs = new List<int>();
            RecognizedTitlesIDs = new List<int>();
            AccountTypeIDs = new List<int>();
            AccountIDs = new List<string>();
        }

    }
}
