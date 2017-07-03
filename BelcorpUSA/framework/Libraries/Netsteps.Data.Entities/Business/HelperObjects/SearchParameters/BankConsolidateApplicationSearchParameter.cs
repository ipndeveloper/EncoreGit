using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class BankConsolidateApplicationSearchParameter : FilterDateRangePaginatedListParameters<BankConsolidateApplicationSearchData>
    {

        static readonly int CHashCodeSeed = typeof(BankConsolidateApplicationSearchParameter).GetKeyForType().GetHashCode();

        public int? Bankid { get; set; }
        public string BankConsolidateDatePro { get; set; }
    }
}
