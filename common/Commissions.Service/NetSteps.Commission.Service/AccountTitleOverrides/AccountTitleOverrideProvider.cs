using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.AccountTitleOverrides
{
	public class AccountTitleOverrideProvider : CommissionsActiveMruCacheAdapter<int, IAccountTitleOverride>, IAccountTitleOverrideProvider
	{
		protected readonly IAccountTitleOverrideRepository Repository;
		
		public AccountTitleOverrideProvider(IAccountTitleOverrideRepository repository)
		{
			Repository = repository;
		}

		protected override ICache<int, IAccountTitleOverride> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, IAccountTitleOverride>("AccountTitleOverride_OverrideItems", new DelegatedDemuxCacheItemResolver<int, IAccountTitleOverride>(GetAccountTitleOverrideItem));
		}

		private bool GetAccountTitleOverrideItem(int key, out IAccountTitleOverride value)
		{
			value = Repository.Fetch(key);
			return value != null;
		}

		public IAccountTitleOverride AddOverride(IAccountTitleOverride accountTitleOverride)
		{
			var added = Repository.Add(accountTitleOverride);
			return this.Get(added.AccountTitleOverrideId);
		}

		public bool DeleteOverride(int accountTitleOverrideId)
		{
			IAccountTitleOverride removed;
			TryRemove(accountTitleOverrideId, out removed);
			return Repository.Delete(accountTitleOverrideId);
		}

		public IAccountTitleOverride UpdateOverride(IAccountTitleOverride accountTitleOverride)
		{
			IAccountTitleOverride removed;
			TryRemove(accountTitleOverride.AccountTitleOverrideId, out removed);
			Repository.Update(accountTitleOverride);
			return this.Get(accountTitleOverride.AccountTitleOverrideId);
		}

		public IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters)
		{
			return Repository.SearchAccountTitleOverrides(parameters);
		}
	}
}
