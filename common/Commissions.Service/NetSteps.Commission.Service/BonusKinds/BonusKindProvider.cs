using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.BonusKind;
using NetSteps.Core.Cache;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.BonusKinds
{
	public class BonusKindProvider : ActiveLocalMemoryCachedListBase<IBonusKind>, IBonusKindProvider
	{
		protected readonly IBonusKindRepository Repository;
		public BonusKindProvider(IBonusKindRepository repository)
		{
			Repository = repository;
		}

		protected override IList<IBonusKind> PerformRefresh()
		{
			return Repository.FetchAll();
		}
	}
}
