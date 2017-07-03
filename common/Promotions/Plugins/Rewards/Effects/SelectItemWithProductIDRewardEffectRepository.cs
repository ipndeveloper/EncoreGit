﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Rewards.Base;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
    [ContainerRegister(typeof(ISelectItemWithProductIDPromotionRewardEffectRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class SelectItemWithProductIDRewardEffectRepository : BasePromotionRewardEffectExtensionRepository<ISelectItemWithProductIDPromotionRewardEffect, PromotionRewardEffectSelectItemWithProductID>, ISelectItemWithProductIDPromotionRewardEffectRepository
    {
    }
}