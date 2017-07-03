using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Override;
using NetSteps.Commissions.Service.Interfaces.OverrideKind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideKinds
{
	public class OverrideKindService : IOverrideKindService
	{
		private IOverrideKindProvider _provider;

		public OverrideKindService(IOverrideKindProvider provider)
		{
			_provider = provider;
		}

		public IEnumerable<IOverrideKind> GetOverrideKinds()
		{
			return _provider.ToArray();
		}
	}
}
