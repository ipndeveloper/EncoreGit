using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_SiteNavigation_Control : Control<Div>
    {
        public TPage ClickLoadSiteInEditMode<TPage>(int? timeout = null) where TPage : NS_Page, new()
        {
            Link lnk = Element.GetElement<Link>(new Param("EditSite", AttributeName.ID.ClassName, RegexOptions.None));
            string[] separators = { "/Login" };
            string url = lnk.GetAttributeValue("href").Split(separators, 2, StringSplitOptions.None)[0];
            lnk.CustomClick(timeout);
            return Util.AttachBrowser<TPage>(url);
        }

        public TPage NavigateLoadSiteInEditMode<TPage>(int? timeout = null) where TPage : NS_Page, new()
        {
            Link lnk = Element.GetElement<Link>(new Param("EditSite", AttributeName.ID.ClassName, RegexOptions.None));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.Browser.Navigate<TPage>(lnk.Url);
        }

         public TPage ReleaseNavigateLoadSiteInEditMode<TPage>(int? timeout = null) where TPage : NS_Page, new()
        {
            Link lnk = Element.GetElement<Link>(new Param("EditSite", AttributeName.ID.ClassName, RegexOptions.None));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.Browser.Navigate<TPage>(lnk.Url.Replace("https", "http"), timeout, true);
        }

         public TPage LoadSiteInEditMode<TPage>(int? timeout = null) where TPage : NS_Page, new()
         {
             Link lnk = Element.GetElement<Link>(new Param("EditSite", AttributeName.ID.ClassName, RegexOptions.None));
             lnk.CustomWaitForExist(timeout: 5);
             return Util.InitBrowser<TPage>(lnk.Url);
         }

        public GMP_Sites_Overview_Page ClickBaseCorporateSite(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Overview/Index/", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Overview_Page>(timeout, pageRequired);
        }
    }
}
