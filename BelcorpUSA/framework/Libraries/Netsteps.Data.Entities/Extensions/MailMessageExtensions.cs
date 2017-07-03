using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: MailMessage Extensions
    /// Created: 04-19-2010
    /// </summary>
    public static class MailMessageExtensions
    {
        static readonly char[] RECIPIENTS_SEPARATOR = new char[] { ';', ' ' };

        public static string ToEmailList(this ObservableCollection<MailMessageRecipient> emailAddresses)
        {
            StringBuilder recipients = new StringBuilder();
            foreach (MailMessageRecipient mailMessageRecipient in emailAddresses)
            {
                //if (!mailMessageRecipient.Email.IsNullOrEmpty())
                {
                    recipients.Append(mailMessageRecipient.AddressFormated).Append(RECIPIENTS_SEPARATOR);
                }
            }

            string result = recipients.ToString().Trim();
            result = result.TrimEnd(RECIPIENTS_SEPARATOR);

            return result;
        }

        public static MailMessage AppendOptOutFooter(this MailMessage mailMessage, string recipientEmail, int languageID)
        {
            // Get the HtmlSection
            HtmlSection section = HtmlSection.LoadFullByTypeAndSectionName(Constants.HtmlSectionEditType.CorporateOnly.ToShort(), "OptOut Email URL");

            // TODO: Filter by SiteID if necessary
            var content = section.HtmlSectionContents
					.OrderByDescending(x => x.HtmlContent.PublishDateUTC ?? DateTime.MinValue)
					.FirstOrDefault(x => x.HtmlContent.LanguageID == languageID &&
                        x.HtmlContent.HtmlContentStatusID == Constants.HtmlContentStatus.Production.ToShort()).HtmlContent;

            if (content != null)
            {
                var htmlElement = content.HtmlElements.FirstOrDefault(x => x.Active == true &&
                                            x.HtmlElementTypeID == Constants.HtmlElementType.Body.ToShort());

                if (htmlElement != null)
                {
                    // Replace the placeholder with the email address
                    string linkContent = htmlElement.Contents.Replace("{0}", recipientEmail);

                    // Insert it within the body tag - if any
                    if (mailMessage.HTMLBody.ContainsIgnoreCase("</body>"))
                    {
                        mailMessage.HTMLBody = mailMessage.HTMLBody.ReplaceIgnoreCase("</body>", linkContent + "</body>");
                    }
                    else
                    {
                        mailMessage.HTMLBody += linkContent;
                    }
                }
            }

            //mailMessage.HTMLBody
            return mailMessage;
        }
    }
}
