using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.DisbursementHolds
{
    public class DisbursementHoldService : IDisbursementHoldService
	{
		protected readonly IDisbursementHoldProvider Provider;
        public DisbursementHoldService(IDisbursementHoldProvider provider)
		{
			Provider = provider;
		}
		public IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold)
		{
			return Provider.AddDisbursementHold(disbursementHold);
		}

		public bool DeleteDisbursementHold(int disbursementHoldId)
		{
            return Provider.DeleteDisbursementHold(disbursementHoldId);
		}

        public IDisbursementHold GetDisbursementHold(int disbursementHoldId)
		{
            return Provider.Get(disbursementHoldId);
		}

        public IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters)
		{
            return Provider.SearchDisbursementHolds(parameters);
		}
	}
}
