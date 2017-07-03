using NetSteps.Accounts.Common.Models;

namespace NetSteps.Events.Common.Repositories
{
	public interface IAccountRepository
	{
		IAccount GetAccountByAccountID(int accountID);
		int GetChildDistributorCount(int accountID);
	}
}
