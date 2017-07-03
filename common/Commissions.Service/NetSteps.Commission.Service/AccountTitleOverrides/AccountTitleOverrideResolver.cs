using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.AccountTitleOverrides
{
	public class AccountTitleOverrideResolver : CacheItemResolver<int, IAccountTitleOverride>
	{
		protected readonly IAccountTitleOverrideRepository Provider;
		public AccountTitleOverrideResolver(IAccountTitleOverrideRepository provider)
		{
			Provider = provider;
		}

		protected override ResolutionKind PerformTryResolve(int key, out IAccountTitleOverride value)
		{
			value = Provider.Fetch(key);
			return value != null ? ResolutionKind.Resolved : ResolutionKind.None;
		}
	}
}
