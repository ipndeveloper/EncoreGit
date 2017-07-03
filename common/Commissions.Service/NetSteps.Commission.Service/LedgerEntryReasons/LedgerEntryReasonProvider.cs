using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.LedgerEntryReasons
{
	public class LedgerEntryReasonProvider : ActiveLocalMemoryCachedListBase<ILedgerEntryReason>, ILedgerEntryReasonProvider
	{
		private ILedgerEntryReasonRepository _repository;

		public LedgerEntryReasonProvider(ILedgerEntryReasonRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ILedgerEntryReason> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
