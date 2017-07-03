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
    [ContainerRegister(typeof(IReduceOrderItemPropertyValuePromotionRewardEffectRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ReduceOrderItemPropertyValueRewardEffectRepository : BasePromotionRewardEffectExtensionRepository<IReduceOrderItemPropertyValuePromotionRewardEffect, PromotionRewardEffectReduceOrderItemPropertyValue>, IReduceOrderItemPropertyValuePromotionRewardEffectRepository
    {
        protected override PromotionRewardEffectReduceOrderItemPropertyValue Convert(IReduceOrderItemPropertyValuePromotionRewardEffect dto)
        {
            var entity = base.Convert(dto);
            if (dto.MarketDecimalOperands != null)
            {
                foreach (var marketID in dto.MarketDecimalOperands.Keys)
                {
                    entity.PromotionRewardEffectReduceOrderItemPropertyValuesMarketValues.Add(new PromotionRewardEffectReduceOrderItemPropertyValuesMarketValue() { MarketID = marketID, DecimalValue = dto.MarketDecimalOperands[marketID] });
                }
            }
            return entity;
        }

        protected override IReduceOrderItemPropertyValuePromotionRewardEffect Convert(PromotionRewardEffectReduceOrderItemPropertyValue entity)
        {
            var extension = base.Convert(entity);
            foreach (var item in entity.PromotionRewardEffectReduceOrderItemPropertyValuesMarketValues)
            {
                extension.MarketDecimalOperands.Add(item.MarketID, item.DecimalValue);
            }
            return extension;
        }
    }
}
