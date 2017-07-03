using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_PersonalWebsite_Page : PWS_Base_Page
    {
        private TextField _website;
        private Link _availability, _next;
        private Span _pwsUrl;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _website = _content.GetElement<TextField>(new Param("Subdomain"));
            _availability = _content.GetElement<Link>(new Param("btnCheckUrlAvailability"));
            _next = _content.GetElement<Link>(new Param("btnSubmit"));
            _pwsUrl = _content.GetElement<Span>(new Param("pwsUrl"));
        }

        public override bool IsPageRendered()
        {
            return _pwsUrl.Exists;
        }

        public string Website
        {
            get { return _website.CustomGetText(); }
            set { SetWebsite(value); }
        }

        public bool ClickCheckAvilability(int? timeout = null)
        {
            bool success = true;
            timeout = _availability.CustomClick(timeout);
            Util.Browser.CustomWaitForComplete(timeout);
            success = _pwsUrl.CustomGetText().Contains(_website.CustomGetText());
            success = _content.GetElement<Span>(new Param("availableMessage")).Exists;
            return success;
        }

        public void SetWebsite(string site)
        {
            _website.CustomSetTextQuicklyHelper(site);
            _website.CustomRunScript(Util.strKeyUp);
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
