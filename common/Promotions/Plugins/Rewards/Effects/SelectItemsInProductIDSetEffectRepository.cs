using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Plugins.EntityModel;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
    [ContainerRegister(typeof(ISelectItemsInProductIDSetPromotionRewardEffectRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class SelectItemsInProductIDSetEffectRepository : BasePromotionRewardEffectExtensionRepository<ISelectItemsInProductIDSetPromotionRewardEffect, PromotionRewardEffectSelectItemsInProductIDSet>, ISelectItemsInProductIDSetPromotionRewardEffectRepository
    {

    }
}
