using System;
using System.Collections.Generic;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.Services
{
    [ContainerRegister(typeof(IAccountAlertConfiguration), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountAlertConfiguration : IAccountAlertConfiguration
    {
        public virtual IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertProvider>>> GetDefaultProviders()
        {
            yield return new KeyValuePair<Guid, Lazy<IAccountAlertProvider>>(
                CommunicationConstants.AccountAlertProviderKey.Promotion,
                new Lazy<IAccountAlertProvider>(() => Create.New<IPromotionAccountAlertProvider>())
            );

            yield return new KeyValuePair<Guid, Lazy<IAccountAlertProvider>>(
                CommunicationConstants.AccountAlertProviderKey.Message,
                new Lazy<IAccountAlertProvider>(() => Create.New<IMessageAccountAlertProvider>())
            );
        }
    }
}
