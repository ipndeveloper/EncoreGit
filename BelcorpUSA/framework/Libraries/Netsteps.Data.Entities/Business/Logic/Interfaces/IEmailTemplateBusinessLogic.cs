using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IEmailTemplateBusinessLogic
    {
        string GeneratePreviewHtml(EmailTemplate emailTemplate, List<EmailTemplateToken> tokens);
        string RemoveLinksForPreview(string htmlBody);
    }
}
