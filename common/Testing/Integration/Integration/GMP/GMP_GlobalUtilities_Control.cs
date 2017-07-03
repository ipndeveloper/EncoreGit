using WatiN.Core;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP
{
    public class GMP_GlobalUtilities_Control : Control<Div>
    {
        public GMP_Login_Page ClickLogOut(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Logout", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout, false);
            return Util.GetPage<GMP_Login_Page>(timeout, pageRequired);
        }

        public TPage SelectMarket<TPage>(Country.ID market) where TPage : GMP_Base_Page, new()
        {
            Link lnk = Element.GetElement<ListItem>(new Param("SubTab Market", AttributeName.ID.ClassName)).GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName))
                .GetElement<Link>(new Param(market.ToPattern(), AttributeName.ID.InnerText, RegexOptions.None));
            if (lnk.Parent.ClassName != "selected")
            {
                // Treat these two lines as atomic
                lnk.CustomClickNoWait(visible: false);
                //Element.CustomWaitForExist();
                Element.GetElement<Link>(new Param("Set Your Market", AttributeName.ID.Title)).GetElement<Span>(new Param(market.ToPattern(), AttributeName.ID.InnerText, RegexOptions.None)).CustomWaitForVisibility(timeout: 120);
                Thread.Sleep(5000);
            }
            return Util.GetPage<TPage>();
        }
    }
}
