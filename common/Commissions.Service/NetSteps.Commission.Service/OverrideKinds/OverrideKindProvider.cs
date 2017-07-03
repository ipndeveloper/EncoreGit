using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideKinds
{
	public class OverrideKindProvider  : ActiveLocalMemoryCachedListBase<IOverrideKind>, IOverrideKindProvider
	{
		protected readonly IOverrideKindRepository _repository;
		public OverrideKindProvider(IOverrideKindRepository repository)
		{
			_repository = repository;
		}

		protected override IList<IOverrideKind> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
