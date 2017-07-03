using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementProfile
{
	public interface IDisbursementProfileProvider : ICache<int, IDisbursementProfile>
	{
		IEnumerable<IDisbursementProfile> GetDisbursementsProfilesForAccount(int accountId);

		int GetMaximumDisbursementProfiles(DisbursementMethodKind method);

		IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile profile);

        string GetDisbursementMethodCode(int disbursementMethodId);
    }
}
