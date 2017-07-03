using System.Collections.Generic;
using NetSteps.Accounts.Common.Models;

namespace NetSteps.Events.Common.Repositories
{
	public interface IAccountPolicyRepository
	{
		IEnumerable<IAccountPolicy> GetAccountPoliciesByAccountID(int accountID);
	}
}
