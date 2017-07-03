using System.Linq;
using NetSteps.Commissions.Service.Interfaces.AccountTitles;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.AccountTitles
{
	public class AccountTitleProvider : IAccountTitleProvider
	{
		protected readonly IAccountTitleRepository Repository;

		private ICache<int, IEnumerable<IAccountTitle>> _accountTitlesByAccountId;
		private ICache<int, IEnumerable<IAccountTitle>> _accountTitlesByPeriodId;

		public AccountTitleProvider(IAccountTitleRepository repository)
		{
			Repository = repository;
			_accountTitlesByAccountId = new ActiveMruLocalMemoryCache<int, IEnumerable<IAccountTitle>>("AccountTitle_AccountIds", new DelegatedDemuxCacheItemResolver<int, IEnumerable<IAccountTitle>>(GetAccountTitlesForAccountId));
			_accountTitlesByPeriodId = new ActiveMruLocalMemoryCache<int, IEnumerable<IAccountTitle>>("AccountTitle_PeriodIds", new DelegatedDemuxCacheItemResolver<int, IEnumerable<IAccountTitle>>(GetAccountTitlesForPeriodId));
		}

		private bool GetAccountTitlesForPeriodId(int key, out IEnumerable<IAccountTitle> value)
		{
			return (value = Repository.GetAccountTitlesForPeriod(key)) != null;
		}

		private bool GetAccountTitlesForAccountId(int key, out IEnumerable<IAccountTitle> value)
		{
			return (value = Repository.Fetch(new int[] { key })) != null;
		}

		public IEnumerable<IAccountTitle> GetAccountTitles(int accountId)
		{
			return _accountTitlesByAccountId.Get(accountId);
		}

		public IEnumerable<IAccountTitle> GetAllAccountTitles()
		{
			return Repository.FetchAll();
		}

		public IEnumerable<IAccountTitle> GetAllAccountTitles(int periodId)
		{
			return _accountTitlesByPeriodId.Get(periodId);
		}
	}
}
