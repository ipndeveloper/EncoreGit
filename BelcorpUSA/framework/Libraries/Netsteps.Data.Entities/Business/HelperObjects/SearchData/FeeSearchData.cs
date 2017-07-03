using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class FeeSearchData
    {
        public int DisbursementFeeID { get; set; }
        public string FeeType { get; set; }
        public string DisbursementTypeTermName { get; set; }
        public string DisbursementType { get; set; }
        public string CurrencyName { get; set; }
        public double Amount { get; set; }
    }
}
