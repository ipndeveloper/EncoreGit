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
	public abstract class SingleMarketStandardQualificationPromotion : StandardQualificationPromotion, ISingleMarketStandardQualificationPromotion
	{
		public SingleMarketStandardQualificationPromotion()
		{
			MarketListExtension = Create.New<IMarketListQualificationExtension>();
		}

		public int MarketID
		{
			get
			{
				if (MarketListExtension == null)
					return 0;
				if (MarketListExtension.Markets.Count > 0)
				{
					return MarketListExtension.Markets.First();
				}
				else
				{
					return 0;
				}
			}
			set
			{
				if (MarketListExtension == null)
				{
					MarketListExtension = Create.New<IMarketListQualificationExtension>();
				}
				MarketListExtension.Markets.Clear();
				MarketListExtension.Markets.Add(value);
			}
		}
	}
}
