using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_TicketEditSectionNav_Control : Control<UnorderedList>
    {
        public GMP_Support_TicketEditDetails_Page ClickTicketDetails(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support/Ticket/Edit/", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Support_TicketEditDetails_Page>(timeout, pageRequired);
        }

        public GMP_Support_TicketHistory_Page ClickTicketHistory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support/Ticket/History/", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Support_TicketHistory_Page>(timeout, pageRequired);
        }
    }
}
