using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_SubNav_Control : Control<UnorderedList>
    {
        public GMP_Support_BrowseTickets_Page ClickBrowseTickets(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Support_BrowseTickets_Page>(timeout, pageRequired);
        }

        public GMP_Support_CreateTicket_Page ClickCreateTicket(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support/Ticket/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Support_CreateTicket_Page>(timeout, pageRequired);
        }
    }
}
