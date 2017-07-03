using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.BonusKind;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Commissions.Service.BonusKinds
{
	public class BonusKindService : IBonusKindService
	{
		protected readonly IBonusKindProvider Provider;
		public BonusKindService(IBonusKindProvider provider)
		{
			Provider = provider;
		}

		public IEnumerable<IBonusKind> GetBonusKinds()
		{
			return Provider;
		}

		public IBonusKind GetBonusKind(int bonusKindId)
		{
			return Provider.SingleOrDefault(x => x.BonusKindId == bonusKindId);
		}

		public IBonusKind GetBonusKind(string bonusCode)
		{
			return Provider.SingleOrDefault(x => x.BonusCode.Equals(bonusCode, StringComparison.InvariantCulture));
		}
	}
}
