using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Support
{
    public class DWS_Support_SectionNav_Control : Control<UnorderedList>
    {
        public DWS_Support_CreateTicket_Page ClickSubmitTicket(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Support/CreateTicket", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Support_CreateTicket_Page>(timeout, pageRequired);
        }
    }
}
