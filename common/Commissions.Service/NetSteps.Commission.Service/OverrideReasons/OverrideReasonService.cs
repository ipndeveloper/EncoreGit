using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.OverrideReasons
{
	public class OverrideReasonService : IOverrideReasonService
	{
		private IOverrideReasonProvider _provider;

		public OverrideReasonService(IOverrideReasonProvider provider)
		{
			_provider = provider;
		}
		public IEnumerable<IOverrideReason> GetOverrideReasons()
		{
			return _provider.ToArray();
		}

		public IEnumerable<IOverrideReason> GetOverrideReasonsForSource(int overrideReasonSourceId)
		{
			return _provider.Where(x => x.OverrideReasonSource.OverrideReasonSourceId == overrideReasonSourceId);
		}
	}
}
