using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Account;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Accounts
{
	public class AccountService : IAccountService
	{
		protected readonly IAccountProvider Provider;
		public AccountService(IAccountProvider provider)
		{
			Provider = provider;
		}

        public bool SaveTemporaryAccountToCommission(int accountId, string accountNumber, int? sponsorId, int? enrollerId)
        {
            if (Provider.Get(accountId) != null)
            {
                return true;
            }

            var account = Create.New<IAccount>();
            account.AccountId = accountId;
            account.AccountNumber = accountNumber;
            account.SponsorId = sponsorId;
            account.EnrollerId = enrollerId;
            account.AccountKindId = 1;
            account.CountryId = 1;
            account.IsEntity = true;

            var savedAccount = Provider.AddAccount(account);
            return savedAccount != null;
        }

        public IAccount AddAccount(IAccount account)
        {
            return Provider.AddAccount(account);
        }

        public IAccount GetAccount(int accountId)
        {
            return Provider.Get(accountId);
        }
    }
}
