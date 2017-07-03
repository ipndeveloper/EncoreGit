using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.ModelConcrete
{
	[Serializable]
	public class PromotionRewardEffect : IPromotionRewardEffect
	{
		public int PromotionRewardEffectID { get; set; }

		public int PromotionRewardID { get; set; }

		public Extensibility.Core.IDataObjectExtension Extension { get; set; }

		public string ExtensionProviderKey { get; set; }
	}
}
