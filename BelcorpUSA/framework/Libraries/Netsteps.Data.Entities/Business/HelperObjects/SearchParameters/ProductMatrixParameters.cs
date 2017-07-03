using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ProductMatrixParameters : FilterDateRangePaginatedListParameters<ProductMatrix>
    {
        public int MaterialID { get; set; }
        public string CUV { get; set; }
        public int LanguageID { get; set; }
    }
}
