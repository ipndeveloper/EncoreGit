using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.LedgerEntryOrigins
{
	public class LedgerEntryOriginProvider : ActiveLocalMemoryCachedListBase<ILedgerEntryOrigin>, ILedgerEntryOriginProvider
	{
		private ILedgerEntryOriginRepository _repository;

		public LedgerEntryOriginProvider(ILedgerEntryOriginRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ILedgerEntryOrigin> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
