using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideReasonSources
{
	public class OverrideReasonSourceService : IOverrideReasonSourceService
	{
		private IOverrideReasonSourceProvider _provider;

		public OverrideReasonSourceService(IOverrideReasonSourceProvider provider)
		{
			_provider = provider;
		}

		public IEnumerable<IOverrideReasonSource> GetOverrideReasonSources()
		{
			return _provider.ToArray();
		}
	}
}
