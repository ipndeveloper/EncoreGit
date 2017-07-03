using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.DisbursementHolds
{
    public class DisbursementHoldResolver : CacheItemResolver<int, IDisbursementHold>
	{
        protected readonly IDisbursementHoldRepository Provider;
        public DisbursementHoldResolver(IDisbursementHoldRepository provider)
		{
			Provider = provider;
		}

        protected override ResolutionKind PerformTryResolve(int key, out IDisbursementHold value)
		{
			value = Provider.Fetch(key);
			return value != null ? ResolutionKind.Resolved : ResolutionKind.None;
		}
	}
}
