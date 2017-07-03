using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;


namespace NetSteps.Data.Entities.Business
{
    public class RenegotiationDetSearchParameters : FilterDateRangePaginatedListParameters<RenegotiationDetSearchData>
    {
        static readonly int CHashCodeSeed = typeof(RenegotiationDetSearchParameters).GetKeyForType().GetHashCode();
        public int RenegotiationConfigurationID { get; set; }
    }
}
