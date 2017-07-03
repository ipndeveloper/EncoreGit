using System;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IMailMessageRecipientEventRepository : IBaseRepository<MailMessageRecipientEvent, Int32>
    {
    }
}
