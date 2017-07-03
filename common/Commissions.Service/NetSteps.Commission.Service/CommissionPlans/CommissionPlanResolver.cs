using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CommissionPlans
{
	public class CommissionPlanResolver : ICacheItemResolver<int, ICommissionPlan>
	{
		private ICommissionPlanRepository _provider;
		public CommissionPlanResolver (ICommissionPlanRepository provider)
		{
			_provider = provider;
		}
		public int AttemptCount
		{
			get { throw new NotImplementedException(); }
		}

		public int ResolveCount
		{
			get { throw new NotImplementedException(); }
		}

		public ResolutionKind TryResolve(int key, out ICommissionPlan value)
		{
			throw new NotImplementedException();
		}
	}
}
