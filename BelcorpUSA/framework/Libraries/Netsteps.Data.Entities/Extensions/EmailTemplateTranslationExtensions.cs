using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Extensions
{
    public static class EmailTemplateTranslationExtensions
    {
        public static MailMessage GetTokenReplacedMailMessage(this EmailTemplateTranslation emailTemplateTranslation, ITokenValueProvider tokenValueProvider)
        {
            return GetTokenReplacedMailMessage(emailTemplateTranslation, tokenValueProvider, Constants.BEGIN_TOKEN_DELIMITER, Constants.END_TOKEN_DELIMITER);
        }

        public static MailMessage GetEmailTemplate(this EmailTemplateTranslation emailTemplateTranslation, ITokenValueProvider tokenValueProvider)
        {
            return GetEmailTemplate(emailTemplateTranslation, tokenValueProvider, Constants.BEGIN_TOKEN_DELIMITER, Constants.END_TOKEN_DELIMITER);
        }

        public static MailMessage GetTokenReplacedMailMessage(this EmailTemplateTranslation emailTemplateTranslation, ITokenValueProvider tokenValueProvider, string beginTokenDelimiter, string endTokenDelimiter)
        {
            var tokenReplacer = new TokenReplacer(tokenValueProvider, beginTokenDelimiter, endTokenDelimiter);
            var mailMessage = new MailMessage();

            if (!string.IsNullOrEmpty(emailTemplateTranslation.Subject))
                mailMessage.Subject = tokenReplacer.ReplaceTokens(emailTemplateTranslation.Subject);
            if (!string.IsNullOrEmpty(emailTemplateTranslation.Body))
                mailMessage.HTMLBody = tokenReplacer.ReplaceTokens(emailTemplateTranslation.Body);
            if (!string.IsNullOrEmpty(emailTemplateTranslation.FromAddress))
                mailMessage.FromAddress = emailTemplateTranslation.FromAddress;

            return mailMessage;
        }

        public static MailMessage GetEmailTemplate(this EmailTemplateTranslation emailTemplateTranslation, ITokenValueProvider tokenValueProvider, string beginTokenDelimiter, string endTokenDelimiter)
        {
            var tokenReplacer = new TokenReplacer(tokenValueProvider, beginTokenDelimiter, endTokenDelimiter);
            var mailMessage = new MailMessage();

            if (!string.IsNullOrEmpty(emailTemplateTranslation.Subject))
                mailMessage.Subject = tokenReplacer.ReplaceTokens(emailTemplateTranslation.Subject);

            if (!string.IsNullOrEmpty(emailTemplateTranslation.Body))
                mailMessage.HTMLBody = string.Format(emailTemplateTranslation.Body, "Juan Perez",1,280,98,96,78);// tokenReplacer.ReplaceTokens(emailTemplateTranslation.Body);

            if (!string.IsNullOrEmpty(emailTemplateTranslation.FromAddress))
                mailMessage.FromAddress = emailTemplateTranslation.FromAddress;

            return mailMessage;
        }

    }
}
