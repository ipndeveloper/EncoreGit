using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public class DWS_Contacts_SectionNavigation_Control : Control<UnorderedList>
    {

        public DWS_Contacts_AddContact_Page ClickAddANewContact(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Contacts/New", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Contacts_AddContact_Page>(timeout, pageRequired);
        }

        public DWS_Contacts_ImportContact_Page ClickImportContact(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Contacts/ImportOutlookContacts", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Contacts_ImportContact_Page>(timeout, pageRequired);
        }

        public DWS_Contacts_Page ClickNewReport(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Contacts/NewReport", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Contacts_Page>(timeout, pageRequired);
        }

    }
}
