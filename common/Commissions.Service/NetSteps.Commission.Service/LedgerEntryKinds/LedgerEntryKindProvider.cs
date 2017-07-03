using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.LedgerEntryKinds
{
	public class LedgerEntryKindProvider : ActiveLocalMemoryCachedListBase<ILedgerEntryKind>, ILedgerEntryKindProvider
	{
		private ILedgerEntryKindRepository _repository;

		public LedgerEntryKindProvider(ILedgerEntryKindRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ILedgerEntryKind> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
