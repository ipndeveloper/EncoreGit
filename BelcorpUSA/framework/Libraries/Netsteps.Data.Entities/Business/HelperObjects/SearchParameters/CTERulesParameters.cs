using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;


namespace NetSteps.Data.Entities.Business
{
    public class CTERulesParameters : FilterDateRangePaginatedListParameters<CTERulesSearchData>
    {
        static readonly int CHashCodeSeed = typeof(CTERulesParameters).GetKeyForType().GetHashCode();
        public int? FineAndInterestRulesID { get; set; }
    }
}
