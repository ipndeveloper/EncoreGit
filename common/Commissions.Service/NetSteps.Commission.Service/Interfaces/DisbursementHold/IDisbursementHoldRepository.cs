using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementHold
{
	/// <summary>
	/// 
	/// </summary>
    public interface IDisbursementHoldRepository : IRepository<IDisbursementHold, int>
	{
        IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters);
	}
}
