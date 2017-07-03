using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class EmailTemplateBusinessLogic
    {
        public virtual string GeneratePreviewHtml(EmailTemplate emailTemplate, List<EmailTemplateToken> tokens)
        {
            string result = emailTemplate.ReplaceTokens(tokens);
            return RemoveLinksForPreview(result);
        }

        public virtual string RemoveLinksForPreview(string htmlBody)
        {
            const string regExpression = @"href=\""(.*?)\""";

            MatchCollection matches = Regex.Matches(htmlBody, regExpression, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
                htmlBody = matches.Cast<Match>().Aggregate(htmlBody, (current, m) => current.Replace(m.ToString(), "href='javascript:void(0);'"));

            return htmlBody;
        }
    }
}
