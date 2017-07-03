using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class TemporalMatrixSearchParameters
    {
        public int ProductPerCatalogID { get; set; }
        public int ProductID { get; set; }
        public int PeriodID { get; set; }
        public string ProductName { get; set; }
        public int NumDeKit { get; set; }
        public bool IsKit { get; set; }
        public int ParticipationKit { get; set; }
        public int SKUSAP { get; set; }
        public int TO { get; set; }
        public decimal PrecioSinDto { get; set; }
        public decimal PrecioMatriz { get; set; }
        public string CodCatalog { get; set; }
        public string Catalog { get; set; }
        public int Points { get; set; }
        public string Type { get; set; }
        public int Page { get; set; }
    }
}
