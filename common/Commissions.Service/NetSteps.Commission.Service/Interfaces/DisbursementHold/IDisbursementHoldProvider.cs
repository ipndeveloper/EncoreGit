using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementHold
{
	/// <summary>
	/// 
	/// </summary>
    public interface IDisbursementHoldProvider : ICache<int, IDisbursementHold>
	{
        IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold);

        bool DeleteDisbursementHold(int disbursementHoldId);

        IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters);
	}
}
