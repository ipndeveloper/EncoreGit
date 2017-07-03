using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;

namespace NetSteps.Commissions.Service.Interfaces.DisbursementProfile
{
	public interface IDisbursementProfileRepository : IRepository<IDisbursementProfile, int>
	{
		IEnumerable<int> GetDisbursementProfileIds(int accountId);
	}
}
