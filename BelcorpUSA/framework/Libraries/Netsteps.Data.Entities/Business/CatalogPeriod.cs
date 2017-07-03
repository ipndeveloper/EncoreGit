using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Entity for Catalog Periods
    /// </summary>
    [Serializable]
    public class CatalogPeriod
    {
        public int CatalogPeriodID { get; set; }
        public int CatalogID { get; set; }
        public int PeriodID { get; set; }
        public string PeriodDescription { get; set; }
    }
}
