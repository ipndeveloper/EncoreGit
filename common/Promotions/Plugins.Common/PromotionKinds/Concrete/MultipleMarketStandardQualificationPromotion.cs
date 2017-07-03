using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete
{
	[Serializable]
	public abstract class MultipleMarketStandardQualificationPromotion : StandardQualificationPromotion, IMultipleMarketStandardQualificationPromotion
	{
		public IEnumerable<int> MarketIDs
		{
			get
			{
				if (MarketListExtension == null)
					return new List<int>();
				return MarketListExtension.Markets;
			}
		}

		public void AddMarketID(int marketID)
		{
			if (MarketListExtension == null)
				MarketListExtension = Create.New<IMarketListQualificationExtension>();
			MarketListExtension.Markets.Add(marketID);
		}

		public void DeleteMarketID(int marketID)
		{
			if (MarketListExtension != null)
			{
				MarketListExtension.Markets.Remove(marketID);
				if (MarketListExtension.Markets.Count == 0)
				{
					MarketListExtension = null;
				}
			}

		}
	}
}
