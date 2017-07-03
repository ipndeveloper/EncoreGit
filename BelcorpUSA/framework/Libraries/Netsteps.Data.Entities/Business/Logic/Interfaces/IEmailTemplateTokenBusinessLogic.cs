using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IEmailTemplateTokenBusinessLogic
    {
        List<string> GetTokenNames();
        List<EmailTemplateToken> GetStandardTokens(Account accountId);
        List<EmailTemplateToken> GetSponsorStandardTokens(Account accountId);
        List<EmailTemplateToken> GetFakeTokensForPreview();
        List<EmailTemplateToken> CombineTokens(List<EmailTemplateToken> defaultTokens, List<EmailTemplateToken> overrideTokens);
        List<EmailTemplateToken> GetPreviewTokens(EmailTemplate emailTemplate, Account account, Account sponsorAccount);
    }
}
