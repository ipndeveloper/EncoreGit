using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideReasons
{
	public class OverrideReasonProvider  : ActiveLocalMemoryCachedListBase<IOverrideReason>, IOverrideReasonProvider
	{
		protected readonly IOverrideReasonRepository _repository;
		public OverrideReasonProvider(IOverrideReasonRepository repository)
		{
			_repository = repository;
		}

		protected override IList<IOverrideReason> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
