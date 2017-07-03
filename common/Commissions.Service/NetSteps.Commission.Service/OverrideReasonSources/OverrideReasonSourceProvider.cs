using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideReasonSources
{
	public class OverrideReasonSourceProvider  : ActiveLocalMemoryCachedListBase<IOverrideReasonSource>, IOverrideReasonSourceProvider
	{
		protected readonly IOverrideReasonSourceRepository _repository;
		public OverrideReasonSourceProvider(IOverrideReasonSourceRepository repository)
		{
			_repository = repository;
		}

		protected override IList<IOverrideReasonSource> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
