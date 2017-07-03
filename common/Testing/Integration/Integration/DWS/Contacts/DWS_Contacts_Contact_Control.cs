using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public class DWS_Contacts_Contact_Control : Control<TableRow>
    {
        private int _index = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeIndex">Cell index of 'Type' cell</param>
        /// <returns></returns>
        public DWS_Contacts_Contact_Control SetTypeIndex(int typeIndex = 0)
        {
            _index = typeIndex;
            return this;
        }

        public string Type()
        {
            return Element.GetElement<TableCell>(new Param(_index)).CustomGetText();
        }

        public string FirstName()
        {
            return Element.GetElement<TableCell>(new Param(_index + 1)).GetElement<Link>().CustomGetText();
        }

        public string LastName()
        {
            return Element.GetElement<TableCell>(new Param(_index + 2)).CustomGetText(); ;
        }

        public string Status()
        {
            return Element.GetElement<TableCell>(new Param(_index + 3)).CustomGetText(); ;
        }

        public string Email()
        {
            return Element.GetElement<TableCell>(new Param(_index + 4)).CustomGetText();
        }

        public DWS_Contacts_ViewContact_Page SelectContact(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<TableCell>(new Param(_index + 1)).GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<DWS_Contacts_ViewContact_Page>(timeout, pageRequired);
        }
    }
}
