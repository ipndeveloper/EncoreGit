using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communication_SectionNav_Control : Control<UnorderedList>
    {
        public DWS_Communication_Compose_Page ClickComposeMessage(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Communication/Email/Compose", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Compose_Page>(timeout, pageRequired);
        }

        public DWS_Communication_Newsletter_Page ClickNewsleter(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Communication/Newsletters", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Newsletter_Page>(timeout, pageRequired);
        }

    }
}
