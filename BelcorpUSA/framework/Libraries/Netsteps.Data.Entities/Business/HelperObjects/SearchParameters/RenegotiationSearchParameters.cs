using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;


namespace NetSteps.Data.Entities.Business
{
    public class RenegotiationSearchParameters : FilterDateRangePaginatedListParameters<RenegotiationSearchData>
    {
        static readonly int CHashCodeSeed = typeof(RenegotiationSearchParameters).GetKeyForType().GetHashCode();
        public string DescriptionRenegotiation { get; set; }
    }
}
