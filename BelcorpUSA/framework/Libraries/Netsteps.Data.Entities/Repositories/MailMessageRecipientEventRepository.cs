using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(IMailMessageRecipientEventRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailMessageRecipientEventRepository : BaseRepository<MailMessageRecipientEvent, int, MailEntities>, IMailMessageRecipientEventRepository, IDefaultImplementation
    {
    }
}
