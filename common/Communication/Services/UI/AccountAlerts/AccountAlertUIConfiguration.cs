using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Communication.UI.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.UI.Services
{
    [ContainerRegister(typeof(IAccountAlertUIConfiguration), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AccountAlertUIConfiguration : IAccountAlertUIConfiguration
    {
        public virtual IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertUIProvider>>> GetDefaultProviders()
        {
            // MessageAccountAlertUI
            yield return new KeyValuePair<Guid, Lazy<IAccountAlertUIProvider>>(
                CommunicationConstants.AccountAlertProviderKey.Message,
                new Lazy<IAccountAlertUIProvider>(() => Create.New<IMessageAccountAlertUIProvider>())
            );
        }
    }
}
