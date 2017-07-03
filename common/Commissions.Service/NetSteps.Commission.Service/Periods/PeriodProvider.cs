using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Periods
{
	public class PeriodProvider : ActiveLocalMemoryCachedListBase<IPeriod>, IPeriodProvider
	{
		protected readonly IPeriodRepository _repository;
		protected readonly ICache<int, IEnumerable<int>> _accountPeriodCache;
		public PeriodProvider(IPeriodRepository repository)
		{
			_repository = repository;
			_accountPeriodCache = new ActiveMruLocalMemoryCache<int, IEnumerable<int>>("Account_PeriodId", new DelegatedDemuxCacheItemResolver<int, IEnumerable<int>>(ResolveAccountPeriodIds));
		}

		protected override IList<IPeriod> PerformRefresh()
		{
			return _repository.FetchAll();
		}

		protected bool ResolveAccountPeriodIds(int accountId, out IEnumerable<int> result)
		{
			return (result = _repository.PeriodIdsForAccount(accountId)) != null;
		}

		public IEnumerable<IPeriod> GetPeriodsForAccount(int accountId)
		{
			var periodIds = _accountPeriodCache.Get(accountId);
			return this.Where(x => periodIds.Contains(x.PeriodId));
		}
	}
}
