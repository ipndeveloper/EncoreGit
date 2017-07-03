using System;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IMailMessageBusinessLogic : IBusinessEntityLogic<MailMessage, Int32, IMailMessageRepository>, IBusinessLogic
    {
        int SaveAsDraft(IMailMessageRepository repository, MailMessage entity, MailAccount mailAccount);
        string ApplyEventTrackingToInternalMessage(string htmlBody, int? campaignActionID, int mailMessageGroupID, string recipientEmailAddress);
        string ApplyEventTrackingToExternalMessage(string htmlBody);
        bool SetReplyToEmailAddress();
    }
}
