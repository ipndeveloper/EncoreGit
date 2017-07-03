using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CommissionPlans
{
	public class CommissionPlanService : ICommissionPlanService
	{
		protected readonly ICommissionPlanProvider _provider;
		public CommissionPlanService(ICommissionPlanProvider provider)
		{
			_provider = provider;
		}
		public ICommissionPlan GetCommissionPlan(DisbursementFrequencyKind disbursementFrequency)
		{
			return _provider.SingleOrDefault(x => x.DisbursementFrequency == disbursementFrequency);
		}

		public ICommissionPlan GetCommissionPlan(int commissionPlanId)
		{
			return _provider.SingleOrDefault(x => x.DisbursementFrequency == (DisbursementFrequencyKind)commissionPlanId);
		}

		public IEnumerable<ICommissionPlan> GetCommissionPlans(Predicate<ICommissionPlan> filter)
		{
			return _provider.Where(x => filter(x));
		}

		public IEnumerable<ICommissionPlan> GetCommissionPlans()
		{
			return _provider.ToArray();
		}
	}
}
