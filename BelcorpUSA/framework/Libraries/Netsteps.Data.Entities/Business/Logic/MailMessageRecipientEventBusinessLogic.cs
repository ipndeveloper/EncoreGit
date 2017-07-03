using System;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
    [ContainerRegister(typeof(IMailMessageRecipientEventBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailMessageRecipientEventBusinessLogic : BusinessLogicBase<MailMessageRecipientEvent, Int32, IMailMessageRecipientEventRepository, IMailMessageRecipientEventBusinessLogic>, IMailMessageRecipientEventBusinessLogic, IDefaultImplementation
    {
    }
}
