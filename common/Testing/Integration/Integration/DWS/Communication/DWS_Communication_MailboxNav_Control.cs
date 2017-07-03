using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communication_MailboxNav_Control : Control<UnorderedList>
    {

        public DWS_Communication_Mailbox_Page ClickInbox(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Inbox", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);
        }

        public DWS_Communication_Mailbox_Page ClickSent(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("SentItems", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);
        }

        public DWS_Communication_Mailbox_Page ClickOutbox(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Outbox", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);
        }

        public DWS_Communication_Mailbox_Page ClickSavedDrafts(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Drafts", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);
        }

        public DWS_Communication_Mailbox_Page ClickTrash(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Trash", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);
        }

        public void ClickNewGroup()
        {
        }
    }
}
