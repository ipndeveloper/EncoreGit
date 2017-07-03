using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_SubNav_Control : Control<UnorderedList>
    {
        public GMP_Communication_Campaigns_Page ClickCampaigns(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>((new Param("/Communication/NewsletterCampaigns", AttributeName.ID.Href, RegexOptions.None))).CustomClick(timeout);
            return Util.GetPage<GMP_Communication_Campaigns_Page>(timeout, pageRequired);
        }

        public GMP_Communication_EmailTemplates_Page ClickEmailTemplates(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Communication/EmailTemplates", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Communication_EmailTemplates_Page>(timeout, pageRequired);
        }

        public GMP_Communication_Alert_page ClickAlerts(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Communication/Alerts", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Communication_Alert_page>(timeout, pageRequired);
        }
    }
}
