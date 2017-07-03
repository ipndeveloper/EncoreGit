using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.AccountTitleOverrides
{
	public class AccountTitleOverrideService : IAccountTitleOverrideService
	{
		protected readonly IAccountTitleOverrideProvider _provider;
		public AccountTitleOverrideService(IAccountTitleOverrideProvider provider)
		{
			_provider = provider;
		}

		public IAccountTitleOverride AddAccountTitleOverride(IAccountTitleOverride accountTitleOverride)
		{
			return _provider.AddOverride(accountTitleOverride);
		}

		public bool DeleteAccountTitleOverride(int overrideId)
		{
			return _provider.DeleteOverride(overrideId);
		}

		public IAccountTitleOverride GetAccountTitleOverride(int overrideId)
		{
			return _provider.Get(overrideId);
		}

		public IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters)
		{
			return _provider.SearchAccountTitleOverrides(parameters);
		}
	}
}
