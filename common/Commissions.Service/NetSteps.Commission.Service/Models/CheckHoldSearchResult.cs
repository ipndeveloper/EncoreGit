using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Models
{
    [System.Serializable]
    public class DisbursementHoldSearchResult : IDisbursementHoldSearchResult
    {
        public IEnumerable<IDisbursementHold> Results { get; set; }

        public int TotalCount { get; set; }
    }
}
