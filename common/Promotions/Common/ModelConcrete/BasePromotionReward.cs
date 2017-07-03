using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Common.ModelConcrete
{
    [Serializable]
    public abstract class BasePromotionReward : IPromotionReward
    {
        public BasePromotionReward()
        {
            Effects = new Dictionary<string, IPromotionRewardEffect>();

        }
        public abstract string PromotionRewardKind { get; }

        public int PromotionRewardID { get; set; }

        public IDictionary<string, IPromotionRewardEffect> Effects { get; private set; }

        public abstract string[] OrderOfApplication { get; }

		protected IPromotionRewardEffect GenerateEffect<TEffectExtension>() where TEffectExtension : IPromotionRewardEffectExtension
		{
			var effect = Create.New<IPromotionRewardEffect>();
			var extension = Create.New<TEffectExtension>();
			effect.Extension = extension;
			effect.ExtensionProviderKey = extension.ExtensionProviderKey;
			return effect;
		}
	}
}
