using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CalculationOverrides
{
	public class CalculationOverrideResolver : CacheItemResolver<int, ICalculationOverride>
	{
		protected readonly ICalculationOverrideRepository Provider;
		public CalculationOverrideResolver(ICalculationOverrideRepository provider)
		{
			Provider = provider;
		}

		protected override ResolutionKind PerformTryResolve(int key, out ICalculationOverride value)
		{
			value = Provider.Fetch(key);
			return value != null ? ResolutionKind.Resolved : ResolutionKind.None;
		}
	}
}
