using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Communication.Services.Models
{
    [ContainerRegister(typeof(IMessageAccountAlert), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class MessageAccountAlert : AccountAlertBase, IMessageAccountAlert
    {
        public string Message { get; set; }

        public MessageAccountAlert()
        {
            ProviderKey = CommunicationConstants.AccountAlertProviderKey.Message;
        }

    }
}
