using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementProfile
{
	public interface IDisbursementProfileService
	{
		int GetDisbursementProfileCountByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod);

		IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile profile);

		IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod);

		IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountId(int accountId);

		int GetMaximumDisbursementProfiles(DisbursementMethodKind method);

        string GetDisbursementMethodCode(int disbursementMethodId);
    }
}
