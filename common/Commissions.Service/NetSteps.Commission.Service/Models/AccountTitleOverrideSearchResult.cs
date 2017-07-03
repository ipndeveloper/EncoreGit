using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
    public class AccountTitleOverrideSearchResult : IAccountTitleOverrideSearchResult
    {
        public IEnumerable<IAccountTitleOverride> Results { get; set; }

        public int TotalCount { get; set; }
    }
}
