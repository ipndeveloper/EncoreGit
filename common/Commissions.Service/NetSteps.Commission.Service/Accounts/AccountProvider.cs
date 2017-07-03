using NetSteps.Commissions.Service.Interfaces.Account;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.Accounts
{
    public class AccountProvider : CommissionsActiveMruCacheAdapter<int, IAccount>, IAccountProvider
	{
		protected readonly IAccountRepository Repository;

		public AccountProvider(IAccountRepository repository)
		{
			Repository = repository;
		}

        protected override ICache<int, IAccount> InitializeCache()
        {
            return new ActiveMruLocalMemoryCache<int, IAccount>("CommissionsAccount_Items", new DelegatedDemuxCacheItemResolver<int, IAccount>(GetItem));
        }

        private bool GetItem(int key, out IAccount value)
        {
            value = Repository.Fetch(key);
            return value != null;
        }

        public IAccount AddAccount(IAccount account)
        {
            var savedAccount = Repository.AddOrUpdate(account);
            return (savedAccount != null) ? this.Get(savedAccount.AccountId) : null;
        }
    }
}
