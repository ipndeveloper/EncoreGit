using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class BlockingSubTypeSearchParameters : FilterDateRangePaginatedListParameters<BlockingSubTypeSearchData>
    {
        public Int16 AccountBlockingSubTypeID { get; set; }
        public Int16 AccountBlockingTypeID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int LanguageID { get; set; }
        public string ListaBlockingProcess { get; set; }
    }

}
