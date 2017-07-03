using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.TitleKinds
{
	public class TitleKindService : ITitleKindService
	{
		protected readonly ITitleKindProvider _provider;
		public TitleKindService(ITitleKindProvider provider)
		{
			_provider = provider;
		}

		public ITitleKind GetTitleKind(int titleKindId)
		{
			return _provider.SingleOrDefault(x => x.TitleKindId == titleKindId);
		}

		public IEnumerable<ITitleKind> GetTitleKinds()
		{
			return _provider.ToArray();
		}
	}
}
