using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Rewards.Base;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
    [ContainerRegister(typeof(IReduceOrderPropertyValuePromotionRewardEffectRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ReduceOrderPropertyValueRewardEffectRepository : BasePromotionRewardEffectExtensionRepository<IReduceOrderPropertyValuePromotionRewardEffect, PromotionRewardEffectReduceOrderPropertyValue>, IReduceOrderPropertyValuePromotionRewardEffectRepository
    {
        protected override PromotionRewardEffectReduceOrderPropertyValue Convert(IReduceOrderPropertyValuePromotionRewardEffect dto)
        {
            var entity = base.Convert(dto);
            if (dto.MarketDecimalOperands != null)
            {
                foreach (var marketID in dto.MarketDecimalOperands.Keys)
                {
                    entity.PromotionRewardEffectReduceOrderPropertyValueMarketValues.Add(new PromotionRewardEffectReduceOrderPropertyValueMarketValue() { MarketID = marketID, DecimalValue = dto.MarketDecimalOperands[marketID] });
                }
            }
            return entity;
        }

        protected override IReduceOrderPropertyValuePromotionRewardEffect Convert(PromotionRewardEffectReduceOrderPropertyValue entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionRewardEffectReduceOrderPropertyValueMarketValues)
            {
                extension.MarketDecimalOperands.Add(item.MarketID, item.DecimalValue);
            }
            return extension;
        }
    }
}
