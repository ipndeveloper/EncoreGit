using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class Header_Control : Control<Div>
    {
        private Div _siteNav;
        protected override void InitializeContents()
        {
            base.InitializeContents();
            _siteNav = Element.GetElement<Div>(new Param("SiteNav"));
        }

        public TPage SelectTab<TPage>(string hrefPattern, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            return SelectTab<TPage>(new Param(hrefPattern, AttributeName.ID.Href, RegexOptions.IgnoreCase), timeout, pageRequired);
        }

        public TPage SelectTab<TPage>(Param parameter, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _siteNav.GetElement<Link>(parameter).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public TMenu GetMenu<TMenu>(string href, int? timeout = null) where TMenu : Menu_Control, new()
        {
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            Link lnk = _siteNav.GetElement<Link>(new Param(href, AttributeName.ID.Href, RegexOptions.None));
            while (lnk.NextSibling.GetType() != typeof(List))
            {
                if (--timeout < 0)
                    throw new TimeoutException();
                System.Threading.Thread.Sleep(1000);
                bool ex = lnk.Exists;
            }
            return lnk.NextSibling.As<TMenu>();
        }
    }
}
