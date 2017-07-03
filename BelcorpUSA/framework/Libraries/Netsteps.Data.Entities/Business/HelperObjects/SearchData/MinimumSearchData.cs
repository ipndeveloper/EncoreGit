using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class MinimumSearchData
    {
        public int DisbursementMinimumID { get; set; }
        public decimal MinimumAmount { get; set; }
        public string CurrencyName { get; set; }
        public string DisbursementTypeTermName { get; set; }
        public string DisbursementType { get; set; }

        // Datos para los combos
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
