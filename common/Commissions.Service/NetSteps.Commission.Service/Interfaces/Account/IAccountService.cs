
using NetSteps.Commissions.Common.Models;
namespace NetSteps.Commissions.Service.Interfaces.Account
{
	public interface IAccountService
	{
        bool SaveTemporaryAccountToCommission(int accountId, string accountNumber, int? sponsorId, int? enrollerId);

	    IAccount AddAccount(IAccount account);

	    IAccount GetAccount(int accountId);
    }
}
