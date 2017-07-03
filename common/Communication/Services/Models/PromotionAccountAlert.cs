using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.Services.Models
{
    [ContainerRegister(typeof(IPromotionAccountAlert), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PromotionAccountAlert : AccountAlertBase, IPromotionAccountAlert
    {
        public int PromotionId { get; set; }

        public PromotionAccountAlert()
        {
            ProviderKey = CommunicationConstants.AccountAlertProviderKey.Promotion;
        }
    }
}
