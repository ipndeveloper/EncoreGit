using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
    public class CalculationOverrideSearchResult : ICalculationOverrideSearchResult
    {
        public IEnumerable<ICalculationOverride> Results { get; set; }

        public int TotalCount { get; set; }
    }
}
