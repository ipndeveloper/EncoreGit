using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class LogisticsProvParameters : FilterDateRangePaginatedListParameters<LogisticsProviderSearData>
    {
        static readonly int CHashCodeSeed = typeof(LogisticsProvParameters).GetKeyForType().GetHashCode();
        public int? LogisticsProviderID { get; set; }
        public string Name { get; set; }

        public int? Active { get; set; }
    }
}
