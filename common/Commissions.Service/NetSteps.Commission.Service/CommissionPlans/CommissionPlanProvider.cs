using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CommissionPlans
{
	public class CommissionPlanProvider : ActiveLocalMemoryCachedListBase<ICommissionPlan>, ICommissionPlanProvider
	{
		protected readonly ICommissionPlanRepository _repository;
		public CommissionPlanProvider(ICommissionPlanRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ICommissionPlan> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
