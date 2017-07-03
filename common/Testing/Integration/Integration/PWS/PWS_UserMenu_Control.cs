using WatiN.Core;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration.PWS.Account;

namespace NetSteps.Testing.Integration.PWS
{
    public class PWS_UserMenu_Control : Control<Div>
    {
        public TPage ClickLogout<TPage>(int? timeout = null, bool pageRequired = true) where TPage : PWS_Base_Page, new()
        {
            Element.GetElement<Link>(new Param("/Logout", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout, false);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Account_Page ClickMyAccount(int? timeout = null)
        {
            timeout = Element.GetElement<Link>(new Param("/Account", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout, false);
            return Util.GetPage<PWS_Account_Page>(timeout);
        }

        public PWS_Account_Page ClickMyWorkstation(int? timeout = null)
        {
            timeout = Element.GetElement<Link>(new Param("http://workstation.jewelkade", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout, false);
            return Util.GetPage<PWS_Account_Page>(timeout);
        }
    }
}
