using System;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IMailMessageRecipientEventBusinessLogic : IBusinessEntityLogic<MailMessageRecipientEvent, Int32, IMailMessageRecipientEventRepository>, IBusinessLogic
    {
    }
}
