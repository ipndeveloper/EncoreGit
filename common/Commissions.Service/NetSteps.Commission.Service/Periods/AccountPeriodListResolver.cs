using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Periods
{
	public class AccountPeriodListResolver : DemuxCacheItemResolver<int, IEnumerable<int>>
	{
		private IPeriodRepository _provider;

		public AccountPeriodListResolver(IPeriodRepository provider)
		{
			_provider = provider;
		}

		protected override bool DemultiplexedTryResolve(int key, out IEnumerable<int> value)
		{
			value =  _provider.PeriodIdsForAccount(key);
			return value.Any();
		}
	}
}
